using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehavior : MonoBehaviour
{
    protected Egg EggToTarget { private set; get; }
    protected Vector3 Target { private set; get; }

    [SerializeField]
    private Renderer[] renderers = new Renderer[0];

    [SerializeField]
    private int blinkForFrames = 8;

    [SerializeField]
    private float blahajDeathThrowbackForce = 130;

    [SerializeField]
    private Gibs gibs;

    protected BlahaijuController blahaiju;
    public NavMeshAgent agent;
    public int lives;

    protected EggsService service;

    private Material[] dynaMats;


    private bool hurtThisFrame = false;
    private int restoreAtFrame;

    public virtual void Initialize(EggsService service, Vector3 spawnPosition, BlahaijuController _blahaiju)
    {
        transform.position = spawnPosition;

        this.service = service;
        EggToTarget = service.Any();
        SetDestination(EggToTarget.transform.position);
        blahaiju = _blahaiju;

        dynaMats = new Material[renderers.Length];
        for (int i = 0; i < renderers.Length; i++)
        {
            dynaMats[i] = Instantiate(renderers[i].sharedMaterial);
            renderers[i].sharedMaterial = dynaMats[i];
        }
    }

    public virtual void Hurt(bool fromBlahaj, bool disableCRSCheck)
    {
        hurtThisFrame = true;

        if (fromBlahaj && blahajDeathThrowbackForce > 0f)
        {
            blahaiju.Bump(
                blahaiju.transform.position - transform.position,
                blahajDeathThrowbackForce
            );
        }

        if (--lives<=0)
        {
            Die();
        }
    }

    public void Die()
    {
        AboutToDie();
        Destroy(gameObject);
    }

    protected virtual void AboutToDie()
    {
        if (gibs)
        {
            gibs.Discobombulate();
        }
    }

    private void OnDestroy()
    {
        for (int i = 0; i < dynaMats.Length; i++)
        {
            Destroy(dynaMats[i]);
        }
    }

    protected virtual void Update()
    {
        if (hurtThisFrame)
        {
            hurtThisFrame = false;
            restoreAtFrame = Time.frameCount + blinkForFrames;
            for (int i = 0; i < dynaMats.Length; i++)
            {
                dynaMats[i].SetColor("_ColorOverride", new Color(1f, 1f, 1f, 1f));
            }
        }
        else if (restoreAtFrame != 0 && restoreAtFrame <= Time.frameCount)
        {
            restoreAtFrame = 0;
            for (int i = 0; i < dynaMats.Length; i++)
            {
                dynaMats[i].SetColor("_ColorOverride", new Color(1f, 1f, 1f, 0f));
            }
        }


        if (service.GameFinished)
        {
            Destroy(this);
            return;
        }

        if (EggToTarget == null)
        {
            EggToTarget = service.Any();
            SetDestination(EggToTarget.transform.position);
        }
    }


    protected virtual void SetDestination(Vector3 destination)
    {
        Target = destination;
        agent.destination = destination;
        transform.LookAt(Target);
    }
}
