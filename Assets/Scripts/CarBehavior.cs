using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarBehavior : EnemyBehavior
{
    public Rigidbody body;
    public float throwbackForce;
    public float villageThrowbackForce = 100;
    public float repathTriggerDistance;
    public float steeringTriggerDistance;
    public float steeringDuration;
    private float steeringTimer;
    private bool isSteering;
    public int incorrectTries;
    public Vector2 angleRange;
    public CollisionEventTransmitter collisionEventTransmitter;
    public List<ParticleSystem> steeringParticles;

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
        //transform.LookAt(agent.destination);
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
            if (--incorrectTries == 0)
            {
                SetCorrectDestination();
            }
            else
            {
                SetIncorrectDestination();
                LaunchSteering();
            }
        }
        else if (!isSteering && agent.remainingDistance < steeringTriggerDistance && incorrectTries>0)
        {
            LaunchSteering();
        }
        if (isSteering)
        {
            UpdateSteering();
        }
        ShiftMovementType();
    }

    void LaunchSteering()
    {
        isSteering = true;
        steeringTimer = 0;
        for (int i = 0; i < steeringParticles.Count; i++)
        {
            steeringParticles[i].Play();
            //steeringParticles[i].gameObject.SetActive(true);
        }
    }

    void UpdateSteering()
    {
        steeringTimer += Time.deltaTime;
        //Do steering
        if (steeringTimer>steeringDuration)
        {
            EndSteering();
        }
    }

    void EndSteering()
    {
        isSteering = false;

        for (int i = 0; i < steeringParticles.Count; i++)
        {
            //steeringParticles[i].gameObject.SetActive(false);
            steeringParticles[i].Stop(false, ParticleSystemStopBehavior.StopEmitting);
        }
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
        base.Hurt(fromBlahaj, disableCRSCheck);
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
                if (enemy.GetType() == typeof(CarBehavior))
                {
                    Hurt(fromBlahaj: false, disableCRSCheck: true);
                    enemy.Hurt(fromBlahaj: false, disableCRSCheck: true);
                }
            }
            else
            {
                throw new System.Exception();
            }
        }
    }
}
