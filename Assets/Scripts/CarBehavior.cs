using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarBehavior : EnemyBehavior
{
    public Rigidbody body;
    public float throwbackForce;
    public float repathTriggerDistance;
    public int incorrectTries;
    public Vector2 angleRange;

    public override void Initialize(Vector3 _target, Vector3 _position, BlahaijuController _blahaiju)
    {
        base.Initialize(_target, _position, _blahaiju);
        SetIncorrectDestination();
        transform.LookAt(agent.destination);
    }

    void SetIncorrectDestination()
    {
        Vector3 direction = eggsTarget - transform.position;
        float angle = Random.Range(angleRange.x, angleRange.y);
        if ((int)Random.Range(0, 2) == 1)
        {
            angle *= -1;
        }
        direction = Quaternion.AngleAxis(angle / direction.magnitude, Vector3.up) * direction;
        Vector3 newTarget = transform.position + direction * 1.3f;
        agent.destination = newTarget;
    }

    void SetCorrectDestination()
    {
        agent.destination = eggsTarget;
    }

    private void Update()
    {
        if (agent.remainingDistance < repathTriggerDistance)
        {
            if (--incorrectTries<=0)
            {
                SetCorrectDestination();
            }
            else
            {
                SetIncorrectDestination();
            }
        }
    }
    public override void Hurt()
    {
        base.Hurt();
        Vector3 direction = transform.position - blahaiju.transform.position;
        body.AddForce(direction.normalized * throwbackForce, ForceMode.Impulse);
    }
}
