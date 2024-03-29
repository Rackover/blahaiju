using LouveSystems.LagOps;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using UnityEngine;

public class BlahaijuController : MonoBehaviour
{
    [SerializeField]
    [Range(0f, 10000f)]
    public float forceAmount = 2000f;

    [SerializeField]
    private EggsService eggs;

    [SerializeField]
    private AnimationCurve precisionOverDistance = AnimationCurve.Constant(0f, 1f, 1F);

    [SerializeField]
    private AnimationCurve forceMultiplierOverDistance = AnimationCurve.Constant(0f, 1f, 1F);

    [SerializeField]
    private CollisionEventTransmitter collisionEventTransmitter;

    [SerializeField]
    [Range(0f, 20f)]
    private float visualTiltOnForce = 40f;

    //[SerializeField]
    //private AnimationCurve visualCatchUpMultiplierWithDistance = AnimationCurve.Linear(0f, 0.2f, 10f, 1F);

    [SerializeField]
    private Transform visualBody;

    //[SerializeField]
    //[Range(0f, 1000f)]
    //private float visualLatency = 0.2f;

    public Rigidbody body;

    private float precisionMultiplier = 0f;
    private float forceMultiplier = 1f;
    private Vector3 destination;

    public void Bump(Vector3 towards, float force)
    {
        body.AddForce(towards.normalized * force, ForceMode.Impulse);
        Camera.main.GetComponent<Hook>()?.ShakeCamera(2, 0.15f);
    }


    private void Awake()
    {
        collisionEventTransmitter.onTriggerEnter += CollisionEventTransmitter_onTriggerEnter;
    }

    private void CollisionEventTransmitter_onTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            var enemy = other.gameObject.GetComponentInParent<EnemyBehavior>();

            if (enemy)
            { 
                enemy.Hurt(fromBlahaj: true, disableCRSCheck: false);
            }
        }
    }

    private void Update()
    {
        if (eggs.GameFinished)
        {
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, LayerMask.GetMask("Default")))
        {
            destination = hit.point;
            Vector3 direction = destination - transform.position;
            float mag = Vector3.Magnitude(direction);
            precisionMultiplier = precisionOverDistance.Evaluate(mag);
            forceMultiplier = forceMultiplierOverDistance.Evaluate(mag);

            float angle = Vector3.Angle(transform.forward, direction.normalized);
            float tilt = visualTiltOnForce * angle;
            visualBody.localEulerAngles = new Vector3(0f, 0f, tilt);
        }
    }

    private void FixedUpdate()
    {
        //Quaternion previousVisualRotation = visualBody.rotation;

        Vector3 directionVector = destination - transform.position;

        Vector3 perfectDirection = directionVector.normalized;
        Vector3 perfectDirectionHorizontal = (destination - transform.position).WithoutY().normalized;

        Vector3 actualDirection = Vector3.Lerp(transform.forward, perfectDirection, precisionMultiplier);
        Vector3 actualDirectionHorizontal = Vector3.Lerp(transform.forward, perfectDirectionHorizontal, precisionMultiplier);
        
        body.AddForce(
            actualDirection * 
            forceAmount *
            forceMultiplier *
            Time.fixedDeltaTime
        );

        body.transform.LookAt(body.transform.position + actualDirectionHorizontal, Vector3.up);

        //float visualCatchUpMultiplier = visualCatchUpMultiplierWithDistance.Evaluate(directionVector.magnitude);

        //visualBody.rotation = Quaternion.Lerp(
        //    previousVisualRotation, 
        //    visualBody.rotation, 
        //    visualLatency * Time.fixedDeltaTime * visualCatchUpMultiplier
        //);
    }
}