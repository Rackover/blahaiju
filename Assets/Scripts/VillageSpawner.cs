using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VillageSpawner : MonoBehaviour
{
    [SerializeField]
    private Village[] villagePrefabs;

    [SerializeField]
    private Transform[] villagePositions;

    [SerializeField]
    private EggsService service;

    private readonly List<Village> villages = new();

    private readonly Queue<Vector3> availablePositions = new();

    private void Awake()
    {
        if (service)
        {
            service.OnDevelopmentIncrease += Service_OnDevelopmentIncrease;
        }
        else
        {
            Debug.LogError($"No service");
            return;
        }

        for (int i = 0; i < villagePositions.Length; i++)
        {
            availablePositions.Enqueue(villagePositions[i].position);
        }

        if (service.maxDevelopment > villagePositions.Where(o => o != null).Count())
        {
            Debug.LogError($"NOT ENOUGH SPOTS FOR VILLAGE SPAWNER !!!! Need {service.maxDevelopment}, currently {villagePositions.Length}");
        }

    }

    private void Service_OnDevelopmentIncrease()
    {
#pragma warning disable IDE0047

        // Skip egg turns
        if (((service.Development / service.spawnEggEveryXDevelopment) * service.spawnEggEveryXDevelopment) != service.Development)
        {
            // Do not spawn too many villages
            if (villages.Count < service.Development / (service.spawnEggEveryXDevelopment - service.spawnVillageEveryXDevelopment))
            {
                SpawnVillage();
            }
        }

#pragma warning restore IDE0047
    }


    private void SpawnVillage()
    {
        Village prefab = villagePrefabs[Random.Range(0, villagePrefabs.Length)];

        Vector3 position = availablePositions.Dequeue();

        Village instance = Instantiate(prefab, transform);
        instance.OnDie += ()=>
        {
            villages.Remove(instance);
            availablePositions.Enqueue(instance.transform.position);
        };

        if (instance)
        {
            instance.transform.position = position;
            instance.transform.eulerAngles = new Vector3(0f, Random.value * 360f, 0f);
        }
    }


#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (villagePositions != null)
        {
            for (int i = 0; i < villagePositions.Length; i++)
            {
                if (villagePositions[i])
                {
                    UnityEditor.Handles.color = Color.white;
                    UnityEditor.Handles.DrawWireDisc(villagePositions[i].position, Vector3.up, 1f);
                }
            }
        }
    }
#endif
}
