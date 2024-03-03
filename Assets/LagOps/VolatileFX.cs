namespace LouveSystems.LagOps
{
    using UnityEngine;
    using DG.Tweening;
    using System.Collections;

    public abstract class VolatileFX : MonoBehaviour
    {
        public virtual bool IsPlaying => isPlaying;

        public float Radius = 10f;

        protected bool isPlaying = false;

        public abstract void Play(float radius);

        public abstract void Play();

        public abstract void End();


        // TODO pooling
        public void ThenDestroy()
        {
            StartCoroutine(DestroyOncefinished());
        }

        IEnumerator DestroyOncefinished()
        {
            while (IsPlaying)
            {
                yield return null;
            }

            Destroy(this.gameObject);
        }
    }
}