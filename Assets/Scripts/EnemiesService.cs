using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesService : MonoBehaviour
{
    public Transform target;
    public float spawnDelay;
    private float spawnTimer;
    private float gameTime;
    public AnimationCurve walkerSpawnCurve;
    public float walkerSpawnCurveEndTime;
    public WalkerBehavior walkerPrefab;
    public Transform spawnPosition;
    public float spawnDistance;

    void Update()
    {
        gameTime += Time.deltaTime;
        spawnTimer += Time.deltaTime;
        if (spawnTimer > spawnDelay)
        {
            spawnTimer -= spawnDelay;
            CheckSpawn();
        }
    }

    void CheckSpawn()
    {
        float random = UnityEngine.Random.Range(0.0f, 1.0f);
        float walkerSpawnChance = walkerSpawnCurve.Evaluate(gameTime / walkerSpawnCurveEndTime);
        if (random < walkerSpawnChance)
        {
            SpawnWalker();
        }
    }

    void SpawnWalker()
    {
        Vector3 spawnPosition = UnityEngine.Random.insideUnitCircle.normalized * spawnDistance;
        spawnPosition = new Vector3(spawnPosition.x, 0, spawnPosition.y);
        WalkerBehavior walker = Instantiate(walkerPrefab);
        walker.Initialize(target.position, spawnPosition);
    }
}
