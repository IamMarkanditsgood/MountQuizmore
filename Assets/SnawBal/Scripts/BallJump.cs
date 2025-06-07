using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BallJump : MonoBehaviour
{
    [SerializeField] private GameObject ballPrefab; // Префаб шарика
    [SerializeField] private float jumpForceMultiplier = 5f; // Сила прыжка
    [SerializeField] private float spawnDelay = 0.1f; // Задержка перед спавном
    [SerializeField] private float ceilingCheckDistance = 0.1f; // Расстояние для проверки потолка
    [SerializeField] private LayerMask ceilingLayer; // Слой, на котором находится потолок

    [SerializeField] private GameObject _loseView;
    [SerializeField] private GameScreen gameScreen;

    private Rigidbody2D rb;
    private float ballHeight;

    private void Start()
    {
        PlayerPrefs.SetInt("CurrentRecord", 0);
        rb = GetComponent<Rigidbody2D>();
        ballHeight = GetComponent<SpriteRenderer>().bounds.size.y; // Определяем высоту шарика
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.touchCount > 0) // ЛКМ или нажатие на экран
        {
            if (!IsCeilingAbove()) // Проверяем, есть ли потолок сверху
            {
                PlayerPrefs.SetInt("CurrentRecord", PlayerPrefs.GetInt("CurrentRecord") + 1);
                StartCoroutine(JumpWithDelay());
            }
        }
    }

    private IEnumerator JumpWithDelay()
    {
        float jumpForce = ballHeight * jumpForceMultiplier;
        Vector2 prevPos = transform.position;

        // Применяем импульс вверх
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);

        // Ждём перед спавном, чтобы главный шарик успел подняться
        yield return new WaitForSeconds(spawnDelay);

        // Спавним новый шарик
        SpawnBall(prevPos);
    }

    private void SpawnBall(Vector3 position)
    {
        Instantiate(ballPrefab, position, Quaternion.identity);
    }

    private bool IsCeilingAbove()
    {
        Vector2 origin = transform.position;
        Vector2 direction = Vector2.up;
        float distance = ballHeight / 2 + ceilingCheckDistance; // Проверяем на небольшом расстоянии над шариком

        RaycastHit2D hit = Physics2D.Raycast(origin, direction, distance, ceilingLayer);
        return hit.collider != null; // Если есть коллайдер сверху, значит потолок мешает
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle") || collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Floor"))
        {
            PlayerPrefs.SetInt("FirstAchieve", 1);
            _loseView.SetActive(true);
            gameScreen.Lose();
            Time.timeScale = 0;
        }
    }
}
