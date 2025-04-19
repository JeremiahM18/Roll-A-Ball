using UnityEngine;
using System.Collections;

public class HeartSpawner : MonoBehaviour
{
    public GameObject heartPrefab;
    public float spawnInterval = 10f;
    public Vector3 spawnAreaSize = new Vector3(20f, 0f, 20f);

    private void Start()
    {
        StartCoroutine(SpawnHearts());
    }

    private IEnumerator SpawnHearts()
    {
        while (true)
        {
            SpawnHeart();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnHeart()
    {
        Vector3 randomPos = new Vector3(
            Random.Range(-spawnAreaSize.x, spawnAreaSize.x), 0f,
            Random.Range(-spawnAreaSize.z, spawnAreaSize.z));

        Instantiate(heartPrefab, randomPos, Quaternion.identity);
    }
}
