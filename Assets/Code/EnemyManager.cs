using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField]
    Transform spawnCenter;
    [SerializeField]
    SpawnConfig config;
    [SerializeField]
    SpawnParams spawnParams;

    float spawnIntervalTimer;
    bool isSpawning;

    public static EnemyManager Instance { get; private set; }
    public SpawnConfig Config => config;

    private void Awake()
    {
        if (Instance != null)
            Destroy(Instance);

        Instance = this;
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
        Enemy enemy = Instantiate(DeterminePrefab());
        enemy.transform.position = position;

        return enemy;
    }

    Enemy DeterminePrefab()
    {
        return spawnParams.Data[0].prefab;
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

    [Serializable]
    public class SpawnParams
    {
        [SerializeField]
        SpawnData[] data;

        public SpawnData[] Data => data;

        public float GetTotalWeight(float factor)
        {
            float result = 0;

            for (int i = 0; i < data.Length; i++)
                result += data[i].CurrentWeight(factor);

            return result;
        }

        [Serializable]
        public struct SpawnData
        {
            public AnimationCurve spawnWeight;
            public Enemy prefab;

            public float CurrentWeight(float factor) =>
                spawnWeight.Evaluate(factor);
        }
    }

    [Serializable]
    public class SpawnConfig
    {
        public float minInterval = 5,
            maxInterval = 8,
            minCount = 1,
            maxCount = 3,
            minDuration = 2.5f,
            maxDuration = 5,
            spawnRadius = 10;
    }
}