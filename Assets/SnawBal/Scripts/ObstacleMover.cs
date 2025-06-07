using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleMover : MonoBehaviour
{
    [SerializeField] private float speed = 2f; // Скорость движения

    private void Update()
    {
        // Двигаем объект влево
        transform.position += Vector3.left * speed * Time.deltaTime;
    }
}
