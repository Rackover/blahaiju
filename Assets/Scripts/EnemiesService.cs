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
    public List<EnemyBehavior> activeEnemies;
    public PoliticianBehavior politicianPrefab;
    public float endGameStartTime;
    private bool endGameStarted;
    public float spawnDistance;
    public float politicianSpawnDistance;
    public bool canSpawn;


    void Update()
    {
        if (eggs.GameFinished || !canSpawn)
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
        if (gameTime > endGameStartTime && !endGameStarted)
        {
            endGameStarted = true;
            SpawnEnemy(politicianPrefab);
        }
    }

    void CheckSpawn()
    {
        float random = 0;
        float enemySpawnChance = 0;
        for (int i = 1; i < enemyProfiles.Count; i++)
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
        float currentSpawnDistance = _prefab.GetType() == typeof(PoliticianBehavior) ? politicianSpawnDistance : spawnDistance;
        Vector3 spawnPosition = new Vector3(Mathf.Sin(rand), 0f, Mathf.Cos(rand)) * currentSpawnDistance;
        EnemyBehavior walker = Instantiate(_prefab);
        activeEnemies.Add(walker);

        walker.Initialize(eggs, spawnPosition, blahaiju, this);
    }

    public void RetireEnemy(EnemyBehavior _enemy)
    {
        activeEnemies.Remove(_enemy);
        if (activeEnemies.Count == 0 && !canSpawn)
        {
            eggs.Win();
        }
    }

    public void PreventSpawn(bool _canSpawn)
    {
        canSpawn = _canSpawn;
    }
}
