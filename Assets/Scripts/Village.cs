
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Village : MonoBehaviour
{
    public event System.Action OnDie;

    [SerializeField]
    private GameObject[] visualElements;

    [SerializeField]
    private int size = 3;

    [SerializeField]
    private float maxRange = 10f;

    [SerializeField]
    private float minRange = 1f;

    private readonly List<GameObject> managedObjects =  new List<GameObject>();

    [SerializeField]
    private CollisionEventTransmitter villageCollider;

    private void Start()
    {
        Vector2[] positions = new Vector2[size];

        positions[0] = Random.insideUnitCircle * (minRange + (maxRange - minRange) * Random.value);
        float previousAngle = Mathf.Atan2(positions[0].x, positions[0].y);
        float maxAmplitude = (1f / size) * Mathf.PI * 2f;
        for (int i = 1; i < size; i++)
        {
            float additive = previousAngle + maxAmplitude * Random.value;
            previousAngle = additive;
            positions[i] = positions[0] + new Vector2(
                Mathf.Sin(additive),
                Mathf.Cos(additive)
            ) * (minRange + (maxRange - minRange) * Random.value);
        }

        managedObjects.Clear();
        for (int i = 0; i < size; i++)
        {
            GameObject prefab = visualElements[Random.Range(0, visualElements.Length)];
            GameObject inst = Instantiate(prefab, transform);
            inst.transform.localPosition = new Vector3(positions[i].x, 0f, positions[i].y);
            inst.transform.localEulerAngles = new Vector3(0f, Random.value * 360f, 0f);
            inst.transform.localScale = Vector3.zero;
            inst.transform.DOScale(1f, 1f + 0.25f * i);
            inst.name = $"visual element {i} ({positions[i]})";
            managedObjects.Add(inst);
        }

        villageCollider.onColliderEnter += VillageCollider_onColliderEnter;
    }

    private void VillageCollider_onColliderEnter(Collision obj)
    {
        if (obj.gameObject.tag == "Enemy")
        {
            var car = obj.gameObject.GetComponentInParent<CarBehavior>();
            if (car)
            {

                Die();

                Vector3 direction = car.transform.position - transform.position;

                car.body.AddForce(direction.normalized * car.villageThrowbackForce, ForceMode.Impulse);
                return;
            }
        }
    }

    private void Die()
    {
        // Todo play FX
        OnDie?.Invoke();
        Destroy(gameObject);
    }
}
