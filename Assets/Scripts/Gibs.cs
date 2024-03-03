using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gibs : MonoBehaviour
{
    [SerializeField]
    private Renderer[] gibs;

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private GameObject bloodFX;

    [SerializeField]
    [Range(0f, 1f)]
    private float chanceToKeepGib = 0.8f;

    [SerializeField]
    [Range(0f, 1f)]
    private float chanceToBleed = 0.5f;

    public void Discobombulate()
    {
        bool hasLostOneMember = false;

        Destroy(animator);
        for (int i = 0; i < gibs.Length; i++)
        {
            var gib = gibs[i];

            if (Random.value <= chanceToKeepGib)
            {
                var sphere = gib.gameObject.AddComponent<SphereCollider>();

                if (gib is SkinnedMeshRenderer sk)
                {
                    sphere.center = sk.bounds.center - sk.transform.position;
                }

                gib.gameObject.layer = LayerMask.NameToLayer("EnemyOnly");

                var body = gib.gameObject.AddComponent<Rigidbody>();
                body.mass = 1f;

                float gibTime = 2f;

                if (!hasLostOneMember || Random.value <= chanceToBleed)
                {
                    hasLostOneMember = true;
                    GameObject fx = Instantiate(bloodFX, gib.transform);
                    fx.transform.localPosition = Vector3.zero;
                    ParticleSystem ps = fx.GetComponent<ParticleSystem>();
                    Camera.main.GetComponent<Hook>().StartCoroutine(KillFXAfterTime(ps, gibTime-0.1f));
                }

                gib.transform.parent = null;
                Destroy(gib.gameObject, gibTime);
            }
        }
    }

    IEnumerator KillFXAfterTime(ParticleSystem ps, float time)
    {
        yield return new WaitForSeconds(time);
        ps.transform.parent = null;
        ps.Stop(withChildren: false, ParticleSystemStopBehavior.StopEmitting);
        Destroy(ps.gameObject, ps.main.startLifetime.constant);
    }
    
}
