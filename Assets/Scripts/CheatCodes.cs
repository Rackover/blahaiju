using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatCodes : MonoBehaviour
{
    [SerializeField]
    private EggsService eggs;

    public BlahaijuController blahaiju;
    public Vector3 target;
    public EnemyBehavior enemyPrefab;
    public CarBehavior carPrefab;
    public CRSBehavior crsPrefab;
    public PoliticianBehavior politicianPrefab;
    public float spawnDistance;
    public float politicianSpawnDistance;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            SpawnEnemy();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            SpawnCar();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            SpawnRiotShield();
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            SpawnPolitician();
        }
    }

    void SpawnPolitician()
    {
        Vector3 spawnPosition = UnityEngine.Random.insideUnitCircle.normalized * politicianSpawnDistance;
        spawnPosition = new Vector3(spawnPosition.x, 0, spawnPosition.y);
        PoliticianBehavior walker = Instantiate(politicianPrefab);
        walker.Initialize(eggs, spawnPosition, blahaiju);
    }

    void SpawnRiotShield()
    {
        Vector3 spawnPosition = UnityEngine.Random.insideUnitCircle.normalized * spawnDistance;
        spawnPosition = new Vector3(spawnPosition.x, 0, spawnPosition.y);
        CRSBehavior walker = Instantiate(crsPrefab);
        walker.Initialize(eggs, spawnPosition, blahaiju);
    }

    void SpawnCar()
    {
        Vector3 spawnPosition = UnityEngine.Random.insideUnitCircle.normalized * spawnDistance;
        spawnPosition = new Vector3(spawnPosition.x, 0, spawnPosition.y);
        CarBehavior walker = Instantiate(carPrefab);
        walker.Initialize(eggs, spawnPosition, blahaiju);
    }

    void SpawnEnemy()
    {
        Vector3 spawnPosition = UnityEngine.Random.insideUnitCircle.normalized * spawnDistance;
        spawnPosition = new Vector3(spawnPosition.x, 0, spawnPosition.y);
        EnemyBehavior walker = Instantiate(enemyPrefab);
        walker.Initialize(eggs, spawnPosition, blahaiju);
    }
}
