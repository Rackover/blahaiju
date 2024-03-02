
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

    private void Start()
    {
        Vector2[] positions = new Vector2[size];

        positions[0] = Random.insideUnitCircle;
        float previousAngle = Mathf.Asin(positions[0].x);
        float maxAmplitude = (1f / size) * Mathf.PI * 2f;
        for (int i = 1; i < size; i++)
        {
            float additive = previousAngle + maxAmplitude * Random.value;
            previousAngle = additive;
            positions[i] = positions[i - 1] + new Vector2(
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
            managedObjects.Add(inst);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            // Todo play FX
            OnDie?.Invoke();
            Destroy(this);
        }
    }
}
