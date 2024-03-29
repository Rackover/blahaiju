using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EggsService : MonoBehaviour
{
    public EnemiesService enemiesService;
    public EndPanel endPanel;

    [SerializeField]
    private AudioClip eggLossClip;

    public int Development => development;

    public bool GameFinished { private set; get; } = false;

    public event Action OnDevelopmentIncrease;

    public int spawnEggEveryXDevelopment = 5;
    public int spawnVillageEveryXDevelopment = 1;
    public int startEggs = 2;

    [SerializeField]
    private float developEveryXSeconds = 15;

    private float developmentTimer;


    private int development;
    public int maxDevelopment;

    [SerializeField]
    private Egg eggPrefab;

    [SerializeField]
    private Transform[] eggSpots;
    
    private readonly Queue<Vector3> availableEggSpots = new Queue<Vector3>();

    private readonly List<Egg> activeEggs = new List<Egg>();

    void InitializeEggs()
    {
        for (int i = 0; i < eggSpots.Length; i++)
        {
            availableEggSpots.Enqueue(eggSpots[i].position);
        }

        for (int i = 0; i < startEggs; i++)
        {
            SpawnEgg();
        }

        if (eggSpots.Length < maxDevelopment/spawnEggEveryXDevelopment)
        {
            Debug.LogError($"Need more egg spots!! Got only ${eggSpots.Length} and expected ${maxDevelopment / spawnEggEveryXDevelopment}");
        }
    }

    private void Awake()
    {
        InitializeEggs();
    }

    void Update()
    {
        if (GameFinished)
        {
            return;
        }

        developmentTimer += Time.deltaTime;
        if (developmentTimer>developEveryXSeconds)
        {
            developmentTimer-=developEveryXSeconds;

            IncrementDevelopment();
        }
    }

    void SpawnEgg()
    {
        if (availableEggSpots.Count == 0)
        {
            Debug.LogError($"NO AVAILABLE EGG SPOTS !! CANNOT SPAWN EGG! !!");
            return;
        }

        var egg = Pooler.DePool(this, eggPrefab);

        egg.transform.parent = transform;
        egg.transform.position = availableEggSpots.Dequeue();

        egg.eggsService = this;

        egg.gameObject.SetActive(true);

        activeEggs.Add(egg);
    }

    void IncrementDevelopment()
    {
        int previousDevelopment = development;
        development++;
        if (development>=maxDevelopment)
        {
            //Win
            enemiesService.PreventSpawn(false);
        }
        else
        {
            if (previousDevelopment/spawnEggEveryXDevelopment < development/spawnEggEveryXDevelopment)
            {
                // time to spawn egg
                SpawnEgg();
            }

            OnDevelopmentIncrease?.Invoke();
        }
    }

    public void Win()
    {
        GameFinished = true;
        endPanel.gameObject.SetActive(true);
        endPanel.Initialize(true);
    }

    public Egg Any()
    {
        return activeEggs[UnityEngine.Random.Range(0, activeEggs.Count)];
    }

    public void LoseEgg(Egg egg)
    {
        Camera.main.GetComponent<AudioSource>().PlayOneShot(eggLossClip);

        Debug.Log($"Lost egg ${egg}");
        if (development >= maxDevelopment)
        {
            enemiesService.PreventSpawn(true);
        }

        if (activeEggs.Remove(egg))
        {
            availableEggSpots.Enqueue(egg.transform.position);

            Pooler.Pool(this, egg);

            development = (development / spawnEggEveryXDevelopment - 1) * spawnEggEveryXDevelopment;

            development = Mathf.Max(0, development);

            Camera.main.GetComponent<Hook>()?.ShakeCamera(10, 1f);

            if (activeEggs.Count == 0)
            {
                // Lost!
                Lose();
            }
        }
    }
    public void Lose()
    {
        Debug.LogError($"Game over!");

        GameFinished = true;
        endPanel.gameObject.SetActive(true);
        endPanel.Initialize(false);
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (eggSpots != null)
        {
            for (int i = 0; i < eggSpots.Length; i++)
            {
                if (eggSpots[i])
                {
                    UnityEditor.Handles.color = Color.yellow;
                    UnityEditor.Handles.DrawWireDisc(eggSpots[i].position, Vector3.up, 1f);
                }
            }
        }
    }
#endif
}
