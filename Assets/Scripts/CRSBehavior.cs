using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CRSBehavior : EnemyBehavior
{
    public override EnemyType Type => EnemyType.CRS;

    public Rigidbody body;
    public float throwbackForce;
    public float distanceToAim;
    public float runSpeed;
    public float slowSpeed;
    public float distanceToSlowDown;

    protected override void Update()
    {
        base.Update();

        if (service.GameFinished)
        {
            return;
        }

        if (Vector3.Distance(transform.position, blahaiju.transform.position) < distanceToAim)
        {
            agent.destination = blahaiju.transform.position;
        }
        else if (agent.destination != Target)
        {
            agent.destination = Target;
        }
        if (agent.remainingDistance < distanceToSlowDown)
        {
            if (agent.speed != slowSpeed)
            {
                agent.speed = slowSpeed;
            }
        }
        else if (agent.speed != runSpeed)
        {
            agent.speed = runSpeed;
        }

        ShiftMovementType();
    }
    void ShiftMovementType()
    {
        if (body.velocity.magnitude > 0 && !agent.isStopped)
        {
            agent.isStopped = true;
        }
        else if (body.velocity.magnitude < 1.0f && agent.isStopped)
        {
            body.velocity = Vector3.zero;
            agent.isStopped = false;
        }
    }

    public override void Hurt(bool fromBlahaj, bool disableCRSCheck)
    {
        //RaycastHit hit;
        Vector3 direction = transform.position - blahaiju.transform.position;
        if (!disableCRSCheck && Vector3.Dot(direction.normalized, transform.forward) < 0)
        {
            body.AddForce(direction.normalized * throwbackForce, ForceMode.Impulse);
        }
        else
        {
            base.Hurt(fromBlahaj, disableCRSCheck);
        }
        //if (Physics.Raycast(blahaiju.transform.position, direction, out hit))
        //{
        //    if (hit.collider.gameObject.CompareTag("Shield"))
        //    {
        //    }
        //    else
        //    {
        //    }
            
        //}
    }
}
