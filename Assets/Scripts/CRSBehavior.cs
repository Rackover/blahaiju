using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CRSBehavior : EnemyBehavior
{
    public Rigidbody body;
    public float throwbackForce;
    public float distanceToAim;

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
    }

    public override void Hurt()
    {
        RaycastHit hit;
        Vector3 direction = transform.position - blahaiju.transform.position;
        if (Physics.Raycast(blahaiju.transform.position, direction, out hit))
        {
            if (hit.collider.gameObject.CompareTag("Shield"))
            {
                body.AddForce(direction.normalized * throwbackForce, ForceMode.Impulse);
            }
            else
            {
                base.Hurt();
            }
            
        }
    }
}
