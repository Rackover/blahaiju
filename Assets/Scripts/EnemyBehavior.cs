using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehavior : MonoBehaviour
{
    protected BlahaijuController blahaiju;
    protected Vector3 eggsTarget;
    public NavMeshAgent agent;
    public int lives;

    public virtual void Initialize(Vector3 _target, Vector3 _position, BlahaijuController _blahaiju)
    {
        eggsTarget = _target;
        agent.destination = _target;
        transform.LookAt(eggsTarget);
        transform.position = _position;
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
}
