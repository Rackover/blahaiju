namespace LouveSystems.LagOps
{
    using UnityEngine;
    using DG.Tweening;

    public class ExplosionFX : VolatileFX
    {
        public override bool IsPlaying => base.IsPlaying || (boomShuriken?.IsAlive() ?? false);

        public float StartRadius = 4f;
        public float StartIntensity = 60f;
        public float EndIntensity = 10f;
        public float StartLightRange = 5f;
        public float EndLightRange = 18f;

        [SerializeField]
        new private Light light;

        //[SerializeField]
        //private Shapes.Disc disc;

        [SerializeField]
        private MeshRenderer explosionBall;

        [SerializeField]
        private ParticleSystem boomShuriken;

        [SerializeField]
        private float duration = 1f;

        [SerializeField]
        AudioClip sound;

        private Material matInstance;
        private Color discColor;
        private Sequence currentSequence;

        private void Awake()
        {
            //discColor = disc.Color;
            matInstance = Instantiate(explosionBall.sharedMaterial);
            explosionBall.sharedMaterial = matInstance;

            End();

            Play();
            ThenDestroy();
        }


        public override void Play(float explosionRadius)
        {
            explosionRadius *= 2f; // We use diameter here instead of radius
            StartRadius = Mathf.Clamp(explosionRadius * 0.4f, 0.5f, 4f);
            Radius = Mathf.Max(explosionRadius, 1f);
            EndLightRange = explosionRadius * 1.8f;
            Play();
        }


        [ContextMenu("Play")]
        public override void Play()
        {
            if (isPlaying)
            {
                End();
            }

            Initialize();
            Camera.main.GetComponent<AudioSource>().PlayOneShot(sound);

            transform.up = Vector3.up;

            var duration = this.duration / Time.timeScale;

            currentSequence = DOTween.Sequence().Pause();

            currentSequence.Append(DOTween.To(() => light.range, x => light.range = x, EndLightRange, duration).Pause());
            currentSequence.Join(DOTween.To(() => light.intensity, x => light.intensity = x, EndIntensity, duration).Pause());
            currentSequence.Join(DOTween.To(() => matInstance.GetFloat("_FillAmount"), x => matInstance.SetFloat("_FillAmount", x), 0.1f, duration).Pause());
            currentSequence.Join(DOTween.To(() => matInstance.GetFloat("_EmissiveMultiplier"), x=> matInstance.SetFloat("_EmissiveMultiplier", x), 5, duration).Pause());
            currentSequence.Join(explosionBall.transform.DOScale(Radius, duration).Pause());
            //currentSequence.Join(DOTween.To(() => disc.Color.a, x => disc.Color = new Color(discColor.r, discColor.g, discColor.b, x), 0, duration * 0.77f).Pause());
            //currentSequence.Join(DOTween.To(() => disc.Radius, x => disc.Radius = x, Radius * 1.5f, duration * 0.77f).Pause());

            boomShuriken.Play();

            currentSequence.AppendCallback(End);

            currentSequence.Play();
        }

        void Initialize()
        {
            isPlaying = true;
            light.intensity = StartIntensity;
            light.range = StartLightRange;
            light.enabled = true;
            explosionBall.transform.localScale = Vector3.one * StartRadius;
            matInstance.SetFloat("_FillAmount", 1);
            matInstance.SetFloat("_EmissiveMultiplier", 1);
            //disc.Radius = 0;
            //disc.Color = discColor;
        }

        public override void End()
        {
            isPlaying = false;
            light.enabled = false;
            explosionBall.transform.localScale = Vector3.zero;
            //disc.Color = new Color();
            currentSequence?.Kill();
        }

        private void OnDestroy()
        {
            Destroy(matInstance);
        }
    }
}