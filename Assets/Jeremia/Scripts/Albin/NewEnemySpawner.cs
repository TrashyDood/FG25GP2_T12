using UnityEngine;
using System.Collections;

public class NewEnemySpawner : MonoBehaviour
{
    [Header("Enemy Prefab")]
    public GameObject enemyPrefab;

    [Header("Spawn Settings")]
    public float spawnRadius = 10f;

    [Header("Timing")]
    public float minSpawnInterval = 1f;
    public float maxSpawnInterval = 3f;

    [Header("Group Size")]
    public int minGroupSize = 1;
    public int maxGroupSize = 4;

    private void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    private IEnumerator SpawnLoop()
    {
        while (true)
        {
            // Wait a random time before next wave
            float wait = Random.Range(minSpawnInterval, maxSpawnInterval);
            yield return new WaitForSeconds(wait);

            // Spawn a random number of enemies
            int count = Random.Range(minGroupSize, maxGroupSize + 1);
            for (int i = 0; i < count; i++)
            {
                SpawnEnemy();
            }
        }
    }

    private void SpawnEnemy()
    {
        // Pick random angle on circle
        float angle = Random.Range(0f, Mathf.PI * 2f);
        Vector3 spawnPos = new Vector3(
            Mathf.Cos(angle),
            0f,
            Mathf.Sin(angle)
        ) * spawnRadius;

        // Offset from player position
        spawnPos += transform.position;

        Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
    }
}