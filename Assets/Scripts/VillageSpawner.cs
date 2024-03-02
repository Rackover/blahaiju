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

    private readonly Dictionary<Village, int> villageSpots = new Dictionary<Village, int>();

    private void Awake()
    {
        if (service)
        {

        }
        else
        {
            Debug.LogError($"No service");
            return;
        }

        if (service.maxDevelopment > villagePositions.Where(o=>o != null).Count())
        {
            Debug.LogError($"NOT ENOUGH SPOTS FOR VILLAGE SPAWNER !!!! Need {service.maxDevelopment}, currently {villagePositions.Length}");
        }

    }

    private void Update()
    {
        villages.RemoveAll(o => o == null);

        foreach(var k in villageSpots.Keys.ToArray())
        {
            if (!villages.Contains(k)) { 
                villageSpots.Remove(k);
            }
        }

        if (service)
        {
            if (villages.Count < service.development)
            {
                SpawnVillage();
            }
        }
    }

    private void SpawnVillage()
    {
        Village prefab = villagePrefabs[Random.Range(0, villagePrefabs.Length)];

        int[] takenSpotsIndices = villageSpots.Values.ToArray();
        List<Transform> freeSpots = new();
        for (int i = 0; i < villagePositions.Length; i++)
        {
            if (!villagePositions[i])
            {
                continue;
            }

            if (takenSpotsIndices.Contains(i))
            {
                continue;
            }

            freeSpots.Add(villagePositions[i]);
        }

        // take closest
        Transform spot = freeSpots
            .OrderBy(o => Vector3.SqrMagnitude(transform.position - o.transform.position))
            .First();

        Village instance = Instantiate(prefab, transform);

        if (instance)
        {
            villageSpots[instance] = villagePositions.ToList().IndexOf(spot); // lose 40 frames on this call alone

            instance.transform.position = spot.transform.position;
            instance.transform.rotation = spot.transform.rotation;
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
                    UnityEditor.Handles.DrawWireDisc(villagePositions[i].position, Vector3.up, 1f);
                }
            }
        }
    }
#endif
}
