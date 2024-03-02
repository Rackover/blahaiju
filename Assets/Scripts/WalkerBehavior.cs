using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkerBehavior : MonoBehaviour
{
    public UnityEngine.AI.NavMeshAgent agent;

    public void Initialize(Vector3 _target, Vector3 _position)
    {
        agent.destination = _target;
        transform.position = _position;
    }
}
