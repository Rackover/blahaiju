using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemiesService : MonoBehaviour
{
    [System.Serializable]
    struct EnemyLimit
    {
        public EnemyType type;
        public int count;
    }

    [SerializeField]
    private EggsService eggs;

    [SerializeField]
    private float spawnDelayMultiplier = 1f;

    [SerializeField]
    private AudioClip smallBoom;

    [SerializeField]
    [Range(0f, 1f)]
    private float randomSignChance = 0.2f;

    [SerializeField]
    private AnimationCurve spawnSpeedOverTime = AnimationCurve.Constant(0f, 180f, 1f);

    [SerializeField]
    private EnemyLimit[] maxEnemiesPerType = new EnemyLimit[0];

    public BlahaijuController blahaiju;

    private float spawnTimer;
    private float gameTime;
    public List<EnemyProfile> enemyProfiles;

    public PoliticianBehavior politicianPrefab;
    public float endGameStartTime;
    private bool endGameStarted;
    public float spawnDistance;
    public float politicianSpawnDistance;
    public bool canSpawn;

    private readonly Dictionary<EnemyType, List<EnemyBehavior>> activeEnemies = new Dictionary<EnemyType, List<EnemyBehavior>>();

    private void Awake()
    {
        for (int i = 0; i < maxEnemiesPerType.Length; i++)
        {
            activeEnemies[maxEnemiesPerType[i].type] = new List<EnemyBehavior>();
        }
    }

    private int GetLimitForProfile(EnemyProfile profile)
    {
        for (int i = 0; i < maxEnemiesPerType.Length; i++)
        {
            if (maxEnemiesPerType[i].type == profile.enemyType)
            {
                return maxEnemiesPerType[i].count;
            }
        }

        return 0;
    }

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
        var profile = enemyProfiles[0];

        for (int i = 1; i < enemyProfiles.Count; i++)
        {
            random = UnityEngine.Random.Range(0.0f, 1.0f);
            enemySpawnChance = enemyProfiles[i].spawnCurve.Evaluate(gameTime / enemyProfiles[i].spawnCurveDuration);

            if (random < enemySpawnChance && activeEnemies[enemyProfiles[i].enemyType].Count < GetLimitForProfile(enemyProfiles[i]))
            {
                profile = enemyProfiles[i];
                break;
            }
        }

        int limit = GetLimitForProfile(profile);
        if (activeEnemies[profile.enemyType].Count >= limit)
        {
            Debug.Log($"Skipping spawn for profile {profile} (hit limit of {limit})");
        }
        else
        {
            if (profile.prefabsVariety.Length > 0)
            {
                var filtered = profile.prefabsVariety.Where(o => o).ToArray();
                var pref = filtered[Random.Range(0, filtered.Length - 1)];
                SpawnEnemy(pref);
            }
            else
            {
                SpawnEnemy(profile.prefab);
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
        EnemyBehavior enemy = Instantiate(_prefab);


        enemy.Initialize(eggs, spawnPosition, blahaiju, this);

        if (enemy is WalkerBehavior walker && Random.value < randomSignChance)
        {
            walker.GiveSign();
        }

        activeEnemies[_prefab.Type].Add(enemy);
    }

    public void RetireEnemy(EnemyBehavior _enemy)
    {
        activeEnemies[_enemy.Type].Remove(_enemy);

        Camera.main.GetComponent<AudioSource>().PlayOneShot(smallBoom);

        int totalEnemies = 0;
        foreach(var kv in activeEnemies)
        {
            totalEnemies += kv.Value.Count;
        }

        if (totalEnemies == 0 && !canSpawn)
        {
            eggs.Win();
        }
    }

    public void PreventSpawn(bool _canSpawn)
    {
        canSpawn = _canSpawn;
    }
}
