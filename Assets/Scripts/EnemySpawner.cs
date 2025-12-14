using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float minimumSpawnTime = 1f;
    [SerializeField] private float maximumSpawnTime = 3f;
    [SerializeField] private int maxEnemiesOnScreen = 6;

    private readonly List<GameObject> aliveEnemies = new();

    private void Awake()
    {
        SpawnEnemy();               
        StartCoroutine(SpawnLoop());
    }

    private IEnumerator SpawnLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(
                Random.Range(minimumSpawnTime, maximumSpawnTime)
            );

            CleanupDeadEnemies();

            if (aliveEnemies.Count >= maxEnemiesOnScreen) continue;

            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        GameObject enemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
        aliveEnemies.Add(enemy);
    }

    private void CleanupDeadEnemies()
    {
        for (int i = aliveEnemies.Count - 1; i >= 0; i--)
        {
            if (aliveEnemies[i] == null)
            {
                aliveEnemies.RemoveAt(i);
            }
        }
    }
}
