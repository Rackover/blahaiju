using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarBehavior : EnemyBehavior
{
    public Rigidbody body;
    public float throwbackForce;
    public float blahajDeathThrowbackForce = 80;
    public float villageThrowbackForce = 100;
    public float repathTriggerDistance;
    public int incorrectTries;
    public Vector2 angleRange;
    public CollisionEventTransmitter collisionEventTransmitter;

    private void Awake()
    {
        collisionEventTransmitter.onColliderEnter += CollisionEventTransmitter_onCollisionEnter;
    }

    private void OnDestroy()
    {
        collisionEventTransmitter.onColliderEnter -= CollisionEventTransmitter_onCollisionEnter;
    }

    public override void Initialize(EggsService service, Vector3 position, BlahaijuController _blahaiju)
    {
        base.Initialize(service, position, _blahaiju);
    }

    protected override void SetDestination(Vector3 destination)
    {
        base.SetDestination(destination);
        SetIncorrectDestination();
    }

    void SetIncorrectDestination()
    {
        Vector3 direction = Target - transform.position;
        float angle = Random.Range(angleRange.x, angleRange.y);
        if ((int)Random.Range(0, 2) == 1)
        {
            angle *= -1;
        }

        direction = Quaternion.AngleAxis(angle / direction.magnitude, Vector3.up) * direction;
        Vector3 newTarget = transform.position + direction * 1.3f;

        agent.destination = newTarget;
        transform.LookAt(agent.destination);
    }

    void SetCorrectDestination()
    {
        agent.destination = Target;
        transform.LookAt(Target);
    }

    protected override void Update()
    {
        base.Update();

        if (service.GameFinished)
        {
            return;
        }

        if (agent.remainingDistance < repathTriggerDistance)
        {
            if (--incorrectTries <= 0)
            {
                SetCorrectDestination();
            }
            else
            {
                SetIncorrectDestination();
            }
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

    public override void Hurt()
    {
        base.Hurt();
        Bump();
    }

    private void Bump()
    {
        Vector3 direction = transform.position - blahaiju.transform.position;
        body.AddForce(direction.normalized * throwbackForce, ForceMode.Impulse);
    }

    private void CollisionEventTransmitter_onCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            var enemy = other.gameObject.GetComponentInParent<EnemyBehavior>();

            if (enemy)
            {
                if (enemy.lives > 1)
                {
                    Hurt();
                    enemy.Hurt();
                }
                else
                {
                    enemy.Die();
                    blahaiju.Bump(blahaiju.transform.position - transform.position, blahajDeathThrowbackForce);
                }
            }
            else
            {
                throw new System.Exception();
            }
        }
    }
}
