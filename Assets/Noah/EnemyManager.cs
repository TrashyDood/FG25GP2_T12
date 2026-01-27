using System;
using System.Collections;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField]
    EnemyPool enemyPool;
    [SerializeField]
    Transform spawnCenter;
    [SerializeField]
    EnemySpawnConfig config;

    float spawnIntervalTimer;
    bool isSpawning;

    public static EnemyManager Instance { get; private set; }
    public EnemySpawnConfig Config => config;

    private void Awake()
    {
        if (Instance != null)
            Destroy(Instance);

        Instance = this;
        enemyPool.parent = transform;
    }

    private void Start()
    {
        spawnCenter = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    private void FixedUpdate()
    {
        if (spawnIntervalTimer <= 0 && !isSpawning)
        {
            spawnIntervalTimer = UnityEngine.Random.Range(config.minInterval, config.maxInterval);
            StartCoroutine(
                SpawnEnemies(
                    UnityEngine.Random.Range(config.minDuration, config.maxDuration),
                    (int)UnityEngine.Random.Range(config.minCount, config.maxCount)));
        }
        else
            spawnIntervalTimer -= Time.fixedDeltaTime;
    }

    Vector3 DeterminePosition() =>
        Utils.RandomCircleEdge(config.spawnRadius) + spawnCenter.position;

    Enemy SpawnEnemy(Vector3 position)
    {
        Enemy enemy = enemyPool.FetchObject();
        enemy.transform.position = position;

        return enemy;
    }

    IEnumerator SpawnEnemies(float duration, int count)
    {
        isSpawning = true;
        float interval = duration / count;

        for (int i = 0; i < count; i++)
        {
            SpawnEnemy(DeterminePosition());
            yield return new WaitForSeconds(interval);
        }

        isSpawning = false;
    }
}

[Serializable]
public class EnemyPool : ComponentPool<Enemy>
{
    public Transform parent;

    protected override void OnFetched(Enemy obj)
    {
        base.OnFetched(obj);
        obj.transform.parent = parent;
    }
}

[Serializable]
public class EnemySpawnConfig
{
    public float minInterval = 5,
        maxInterval = 8,
        minCount = 1,
        maxCount = 3,
        minDuration = 2.5f,
        maxDuration = 5,
        spawnRadius = 10;
}