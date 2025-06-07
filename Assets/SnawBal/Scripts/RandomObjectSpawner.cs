using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomObjectSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] spawnPrefabs; // Массив возможных объектов
    [SerializeField] private Transform spawnPoint; // Точка спавна (X,Z фиксированные)
    [SerializeField] private float minY = -3f; // Минимальная Y координата
    [SerializeField] private float maxY = 3f; // Максимальная Y координата
    [SerializeField] private float minSpawnDelay = 1f; // Минимальный интервал спавна
    [SerializeField] private float maxSpawnDelay = 3f; // Максимальный интервал спавна

    private void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            float randomDelay = Random.Range(minSpawnDelay, maxSpawnDelay);
            yield return new WaitForSeconds(randomDelay);

            SpawnObject();
        }
    }

    private void SpawnObject()
    {
        if (spawnPrefabs.Length == 0) return;

        // Выбираем случайный объект
        GameObject randomPrefab = spawnPrefabs[Random.Range(0, spawnPrefabs.Length)];

        // Генерируем случайную позицию по Y
        float randomY = Random.Range(minY, maxY);
        Vector3 spawnPosition = new Vector3(spawnPoint.position.x, randomY, spawnPoint.position.z);

        // Создаём объект
        Instantiate(randomPrefab, spawnPosition, Quaternion.identity);
    }
}
