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
    HashSet<Enemy> enemies = new();

    public static EnemyManager Instance { get; private set; }

    public float GameTimeProgress => GameManager.Instance.GameTimeProgress;

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
        if (spawnIntervalTimer <= 0 && !isSpawning && enemies.Count < spawnConfig.maxEnemyCount)
        {
            StartCoroutine(
                SpawnEnemies(
                    spawnConfig.waveDuration.GetValue(GameTimeProgress),
                    Mathf.CeilToInt(spawnConfig.spawnCount.GetValue(GameTimeProgress))));

            spawnIntervalTimer = spawnConfig.waveInterval.GetValue(GameTimeProgress);
        }
        else
            spawnIntervalTimer -= Time.fixedDeltaTime;
    }

    void OnSpawnConfigChanged(SpawnConfig newConfig) { }

    IEnumerator SpawnEnemies(float duration, int count)
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

    Vector3 DeterminePosition()
    {
        float angle = UnityEngine.Random.Range(0, Mathf.PI * 2);
        return (new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * spawnConfig.spawnRadius) + spawnCenter.position;
    }

    Enemy SpawnEnemy(Vector3 position, Enemy prefab)
    {
        Enemy enemy = Instantiate(prefab, transform);
        enemy.transform.position = position;

        enemies.Add(enemy);
        enemy.OnDeathEvent += () => { enemies.Remove(enemy); };

        return enemy;
    }

    [Serializable]
    public class SpawnParams
    {
        [SerializeField]
        Data[] data;

        float[] weights;

        public IEnumerable<Data> GetEnemies(float factor, int count)
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
        public struct Data
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
        public Value waveInterval,
            waveDuration,
            spawnCount;
        public float spawnRadius = 20;
        public uint maxEnemyCount = 20;


        [Serializable]
        public struct Value
        {
            public AnimationCurve scaling;
            public float min, max;

            public float GetValue(float factor) =>
                UnityEngine.Random.Range(min, max) * scaling.Evaluate(factor);
        }
    }
}