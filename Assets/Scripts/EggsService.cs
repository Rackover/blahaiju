using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggsService : MonoBehaviour
{
    public float eggSpawnDelay;
    private float eggSpawnTimer;
    public int development;
    public int initialDevelopment;
    public int maxDevelopment;
    public List<Egg> eggs;


    void InitializeEggs()
    {
        development = initialDevelopment;
    }

    void Update()
    {
        eggSpawnTimer += Time.deltaTime;
        if (eggSpawnTimer>eggSpawnDelay)
        {
            eggSpawnTimer-=eggSpawnDelay;
            SpawnEgg();
        }
    }

    void SpawnEgg()
    {
        for (int i = 0; i < eggs.Count; i++)
        {
            if (!eggs[i].gameObject.activeSelf)
            {
                eggs[i].gameObject.SetActive(true);
                break;
            }
        }
        IncrementDevelopment();
    }

    void IncrementDevelopment()
    {
        development++;
        if (development>=maxDevelopment)
        {
            //Win
        }
    }

    public void LoseDevelopment()
    {
        development--;
        if (development <= 0)
        {
            //Lose
        }
    }
}
