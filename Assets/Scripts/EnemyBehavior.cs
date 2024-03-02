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


    protected BlahaijuController blahaiju;
    public NavMeshAgent agent;
    public int lives;

    private Material[] dynaMats;

    protected EggsService service;

    private bool hurtThisFrame = false;
    private bool wasHurtPreviousFrame = false;

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

    public virtual void Hurt()
    {
        hurtThisFrame = true;

        if (--lives<=0)
        {
            Die();
        }
    }

    public void Die()
    {
        Destroy(gameObject);
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
            wasHurtPreviousFrame = true;
            for (int i = 0; i < dynaMats.Length; i++)
            {
                dynaMats[i].SetColor("_ColorOverride", new Color(1f, 1f, 1f, 1f));
            }
        }
        else if (wasHurtPreviousFrame)
        {
            wasHurtPreviousFrame = false;
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
