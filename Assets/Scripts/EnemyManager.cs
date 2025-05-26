using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WaveDetails
{
    public int basicEnemyCount;
    public int fastEnemyCount;
}
public class EnemyManager : MonoBehaviour
{
    [SerializeField] private WaveDetails currentWave;
    [Space]
    [SerializeField] private Transform respawnPoint;
    [SerializeField] private float spawnCooldown;
    private float spawnTimer;

    private List<GameObject> enemiesToCreate;

    [Header("Enemy Prefabs")]
    [SerializeField] private GameObject basicEnemy;
    [SerializeField] private GameObject fastEnemy;

    private void Start()
    {
        enemiesToCreate = GetEnemiesToCreate();
    }

    private void Update()
    {
        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0.0f && enemiesToCreate.Count > 0)
        {
            spawnTimer = spawnCooldown;

            GameObject enemyPrefab = GetRandomEnemy();
            SpawnEnemy(enemyPrefab, respawnPoint.position);
        }
    }

    private void SpawnEnemy(GameObject enemyPrefab, Vector3 position)
    {
        if (enemyPrefab != null)
        {
            Instantiate(enemyPrefab, position, Quaternion.identity);
        }
        else
        {
            Debug.LogError("Enemy prefab is not assigned in the inspector.");
        }
    }

    private GameObject GetRandomEnemy()
    {
        if (enemiesToCreate.Count == 0)
        {
            Debug.LogError("No enemies to create. Please check the enemy list.");
            return null;
        }
        int randomIndex = Random.Range(0, enemiesToCreate.Count);
        GameObject enemyPrefab = enemiesToCreate[randomIndex];
        enemiesToCreate.RemoveAt(randomIndex); // Remove the enemy from the list after spawning
        return enemyPrefab;
    }

    private List<GameObject> GetEnemiesToCreate()
    {
        List<GameObject> enemies = new List<GameObject>();
        for (int i = 0; i < currentWave.basicEnemyCount; i++)
        {
            enemies.Add(basicEnemy);
        }
        for (int i = 0; i < currentWave.fastEnemyCount; i++)
        {
            enemies.Add(fastEnemy);
        }
        return enemies;
    }
}
