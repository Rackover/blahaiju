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
    public List<EnemyProfile> enemyProfiles;
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
        float random = 0;
        float enemySpawnChance = 0;
        for (int i = 0; i < enemyProfiles.Count; i++)
        {
            random = UnityEngine.Random.Range(0.0f, 1.0f);
            enemySpawnChance = enemyProfiles[i].spawnCurve.Evaluate(gameTime / enemyProfiles[i].spawnCurveDuration);
            if (random < enemySpawnChance)
            {
                SpawnEnemy(enemyProfiles[i].prefab);
            }
        }
    }

    void SpawnEnemy(EnemyBehavior _prefab)
    {
        Vector3 spawnPosition = UnityEngine.Random.insideUnitCircle.normalized * spawnDistance;
        spawnPosition = new Vector3(spawnPosition.x, 0, spawnPosition.y);
        EnemyBehavior walker = Instantiate(_prefab);
        walker.Initialize(target.position, spawnPosition);
    }
}
