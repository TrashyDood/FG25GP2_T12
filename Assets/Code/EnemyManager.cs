using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyManager : MonoBehaviour
{
    [SerializeField]
    Transform spawnCenter;
    [SerializeField]
    SpawnConfig spawnConfig;
    [SerializeField]
    SpawnParams spawnParams;

    [Header("Events")]
    [SerializeField]
    UnityEvent<SpawnConfig, SpawnConfig> onConfigsChangedEvent;
    
    bool isSpawning;
    float spawnIntervalTimer;

    public static EnemyManager Instance { get; private set; }

    public SpawnConfig Config
    {
        get => spawnConfig;
        set
        {
            OnSpawnConfigChanged(value);
            spawnConfig = value;
        }
    }

    private void Awake()
    {
        if (Instance != null)
            Destroy(Instance);

        Instance = this;
    }

    private void Start() =>
        spawnCenter = GameManager.Instance.Player.transform;

    private void FixedUpdate()
    {
        if (spawnIntervalTimer <= 0 && !isSpawning)
        {
            StartCoroutine(
                SpawnEnemies(
                    UnityEngine.Random.Range(spawnConfig.minDuration, spawnConfig.maxDuration),
                    (uint)UnityEngine.Random.Range(spawnConfig.minCount, spawnConfig.maxCount)));

            spawnIntervalTimer = UnityEngine.Random.Range(spawnConfig.minInterval, spawnConfig.maxInterval);
        }
        else
            spawnIntervalTimer -= Time.fixedDeltaTime;
    }

    void OnSpawnConfigChanged(SpawnConfig newConfig) { }

    IEnumerator SpawnEnemies(float duration, uint count)
    {
        isSpawning = true;
        float interval = duration / count;

        foreach (var enemy in spawnParams.GetEnemies(GameManager.Instance.GameTimeProgress, count))
        {
            SpawnEnemy(DeterminePosition(), enemy.prefab);
            yield return new WaitForSeconds(interval);
        }

        isSpawning = false;
    }

    Vector3 DeterminePosition() =>
        Utils.RandomCircleEdge(spawnConfig.spawnRadius) + spawnCenter.position;

    Enemy SpawnEnemy(Vector3 position, Enemy prefab)
    {
        Enemy enemy = Instantiate(prefab, transform);
        enemy.transform.localPosition = position;

        return enemy;
    }

    [Serializable]
    public class SpawnParams
    {
        [SerializeField]
        SpawnData[] data;

        float[] weights;

        public SpawnData[] Data => data;

        public IEnumerable<SpawnData> GetEnemies(float factor, uint count)
        {
            float totalWeight = 0;

            weights ??= new float[data.Length];

            for (int i = 0; i < data.Length; i++)
                totalWeight += (weights[i] = data[i].CurrentWeight(factor));

            float rand = 0;

            for (uint i = 0; i < count; i++)
            {
                rand = UnityEngine.Random.Range(0, totalWeight);

                for (int j = 0; j < weights.Length; j++)
                {
                    if (rand < weights[j])
                    {
                        yield return data[j];
                        break;
                    }

                    rand -= weights[j];
                }
            }
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
            minDuration = 2.5f,
            maxDuration = 5,
            spawnRadius = 10;
        public uint minCount = 1,
            maxCount = 3;
    }
}