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
}
