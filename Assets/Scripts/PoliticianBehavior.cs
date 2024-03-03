using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliticianBehavior : EnemyBehavior
{
    public override EnemyType Type => EnemyType.Politician;

    [SerializeField]
    private float blahajBumpForce = 130;

    public CollisionEventTransmitter collisionEventTransmitter;

    private void OnEnable()
    {
        collisionEventTransmitter.onColliderEnter += CollisionEventTransmitter_onCollisionEnter;
    }

    private void OnDestroy()
    {
        collisionEventTransmitter.onColliderEnter -= CollisionEventTransmitter_onCollisionEnter;
    }

    private void OnDisable()
    {
        collisionEventTransmitter.onColliderEnter -= CollisionEventTransmitter_onCollisionEnter;
    }

    private void CollisionEventTransmitter_onCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            var enemy = other.gameObject.GetComponentInParent<EnemyBehavior>();

            if (enemy)
            {
                enemy.Hurt(fromBlahaj: false, disableCRSCheck: true);
            }
            else
            {
                throw new System.Exception();
            }
        }
        else if (other.gameObject.tag == "Player")
        {
            blahaiju.Bump(blahaiju.transform.position - transform.position, blahajBumpForce);
        }
    }
}
