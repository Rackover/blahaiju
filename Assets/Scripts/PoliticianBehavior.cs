using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliticianBehavior : EnemyBehavior
{
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
    }
}
