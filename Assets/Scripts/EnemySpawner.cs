using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnInterval = 5f;
    public Vector3 spawnAreaSize = new Vector3(20f, 0f, 20f);
    public Transform player;

    private void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        while (true)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnEnemy()
    {
        Vector3 randomPos = new Vector3(
            Random.Range(-spawnAreaSize.x, spawnAreaSize.x), 0f,
            Random.Range(-spawnAreaSize.z, spawnAreaSize.z));

        GameObject enemy = Instantiate(enemyPrefab, randomPos, Quaternion.identity);

        EnemyMovement enemyMovement = enemy.GetComponent<EnemyMovement>();
        if (enemyMovement != null)
        {
            enemyMovement.player = player;
        }
    }
}

