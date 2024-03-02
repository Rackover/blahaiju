using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehavior : MonoBehaviour
{
    protected Egg EggToTarget { private set; get; }
    protected Vector3 Target { private set; get; }

    protected BlahaijuController blahaiju;
    public NavMeshAgent agent;
    public int lives;

    protected EggsService service;

    public virtual void Initialize(EggsService service, Vector3 spawnPosition, BlahaijuController _blahaiju)
    {
        this.service = service;
        EggToTarget = service.Any();
        SetDestination(EggToTarget.transform.position);

        transform.position = spawnPosition;
        blahaiju = _blahaiju;
    }

    public virtual void Hurt()
    {
        if (--lives<=0)
        {
            Die();
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    protected virtual void Update()
    {
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
        agent.destination = destination;
        transform.LookAt(Target);
    }
}
