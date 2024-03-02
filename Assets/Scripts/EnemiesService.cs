using System.Collections.Generic;
using UnityEngine;

public class EnemiesService : MonoBehaviour
{
    [SerializeField]
    private EggsService eggs;

    [SerializeField]
    private float spawnDelayMultiplier = 1f;

    [SerializeField]
    private AnimationCurve spawnSpeedOverTime = AnimationCurve.Constant(0f, 180f, 1f);

    public BlahaijuController blahaiju;

    private float spawnTimer;
    private float gameTime;
    public List<EnemyProfile> enemyProfiles;
    public float spawnDistance;

    void Update()
    {
        if (eggs.GameFinished)
        {
            return;
        }

        gameTime += Time.deltaTime;
        spawnTimer += Time.deltaTime;

        float spawnDelay = spawnDelayMultiplier * spawnSpeedOverTime.Evaluate(gameTime);

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
            else
            {
                SpawnEnemy(enemyProfiles[0].prefab);
            }
        }
    }

    void SpawnEnemy(EnemyBehavior _prefab)
    {
        if (eggs.GameFinished)
        {
            return;
        }

        float rand = (UnityEngine.Random.value * Time.time % 1f) * Mathf.PI * 2f;
        Vector3 spawnPosition = new Vector3(Mathf.Sin(rand), 0f, Mathf.Cos(rand)) * spawnDistance;
        EnemyBehavior walker = Instantiate(_prefab);

        walker.Initialize(eggs, spawnPosition, blahaiju);
    }
}
