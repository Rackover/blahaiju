using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatCodes : MonoBehaviour
{
    public Vector3 target;
    public EnemyBehavior enemyPrefab;
    public float spawnDistance;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            SpawnEnemy();
        }
    }

    void SpawnEnemy()
    {
        Vector3 spawnPosition = UnityEngine.Random.insideUnitCircle.normalized * spawnDistance;
        spawnPosition = new Vector3(spawnPosition.x, 0, spawnPosition.y);
        EnemyBehavior walker = Instantiate(enemyPrefab);
        walker.Initialize(target, spawnPosition);
    }
}
