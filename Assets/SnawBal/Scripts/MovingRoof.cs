using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingRoof : MonoBehaviour
{
    [SerializeField] private float speed = 2f; // Скорость движения
    [SerializeField] private float resetPositionX = 10f; // Где объект появится заново
    [SerializeField] private float thresholdX = -10f; // Где объект исчезает и переносится назад

    private void Update()
    {
        // Двигаем объект влево
        transform.position += Vector3.left * speed * Time.deltaTime;

        // Если объект ушел за границу, переносим его назад
        if (transform.position.x <= thresholdX)
        {
            ResetPosition();
        }
    }

    private void ResetPosition()
    {
        Vector3 newPosition = transform.position;
        newPosition.x = resetPositionX;
        transform.position = newPosition;
    }
}
