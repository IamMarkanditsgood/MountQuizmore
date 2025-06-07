using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundaryDestroyer : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Obstacle") || collision.CompareTag("Ball"))
        {
            Destroy(collision.gameObject);
        }
        if (collision.CompareTag("PlayerBall"))
        {
            Time.timeScale = 0;
            Destroy(collision.gameObject);
        }
    }
}
