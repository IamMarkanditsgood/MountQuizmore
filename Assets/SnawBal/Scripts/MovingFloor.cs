using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingFloor : MonoBehaviour
{
    [SerializeField] private float speed = 2f; // Скорость движения
    [SerializeField] private float resetPositionX = 10f; // Где объект появится заново
    [SerializeField] private float thresholdX = -10f; // Где объект исчезает и переносится назад
    [SerializeField] private float minY = -2f; // Минимальный Y
    [SerializeField] private float maxY = 2f; // Максимальный Y
    [SerializeField] private float heightStep = 1f; // Шаг изменения Y

    private void Update()
    {
        // Двигаем объект влево
        transform.position += Vector3.left * speed * Time.deltaTime;

        // Если объект ушел за границу, переносим его назад с новым Y
        if (transform.position.x <= thresholdX)
        {
            ResetPosition();
        }
    }

    private void ResetPosition()
    {
        Vector3 newPosition = transform.position;
        newPosition.x = resetPositionX;

        // Выбираем случайный Y с заданным шагом
        int steps = Mathf.FloorToInt((maxY - minY) / heightStep); // Количество возможных шагов
        float randomStep = Random.Range(0, steps + 1) * heightStep; // Выбираем случайный шаг
        newPosition.y = minY + randomStep;

        transform.position = newPosition;
    }
}
