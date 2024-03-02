using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehavior : MonoBehaviour
{
    public NavMeshAgent agent;

    public void Initialize(Vector3 _target, Vector3 _position)
    {
        agent.destination = _target;
        transform.position = _position;
    }

    //void OnCollisionEnter(Collision _col)
    //{ 
    //    if (_col.gameObject.CompareTag("Player"))
    //    {
    //        Hurt();
    //    }
                  
    //}

    public void Hurt()
    {
        Die();
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
