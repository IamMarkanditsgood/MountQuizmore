using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreesSpawner : MonoBehaviour
{
    [SerializeField] private GameObject obstaclePrefab; // Префаб препятствия
    [SerializeField] private Transform spawnPoint; // Точка спавна
    [SerializeField] private float spawnInterval = 2f; // Интервал попыток спавна
    [SerializeField] private float spawnChance = 50f; // Шанс спавна (0-100%)

    private void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            // Проверяем шанс
            if (Random.Range(0f, 100f) <= spawnChance)
            {
                SpawnObstacle();
            }
        }
    }

    private void SpawnObstacle()
    {
        Instantiate(obstaclePrefab, spawnPoint.position, Quaternion.identity);
    }
}
