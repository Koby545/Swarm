using UnityEngine;
using System.Collections;

public class Boss : MonoBehaviour
{
    public float speed = 1.5f;
    public int scoreValue = 100;
    private int health;
    private Transform player;
    private float wobbleTimer;
    public float wobbleSpeed = 3f;
    public float wobbleAmount = 10f;
    private SpriteRenderer sr;
    private Color originalColor;

    public void Init(int waveNumber)
    {
        health = waveNumber * 4;
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        wobbleTimer = Random.Range(0f, 100f);
        sr = GetComponent<SpriteRenderer>();
        originalColor = sr.color;
    }

    void Update()
    {
        if (player == null) return;

        Vector2 direction = (player.position - transform.position).normalized;
        transform.position += (Vector3)direction * speed * Time.deltaTime;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        wobbleTimer += Time.deltaTime * wobbleSpeed;
        float wobble = Mathf.Sin(wobbleTimer) * wobbleAmount;
        transform.rotation = Quaternion.Euler(0, 0, angle  + wobble);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            GameManager.Instance.AddScore(scoreValue);
            GetComponent<DeathEffect>().Explode();
            AudioManager.Instance.PlaySplat();
            Destroy(gameObject);
        }
        StartCoroutine(FlashWhite());
    }

    IEnumerator FlashWhite()
    {
        sr.color = Color.white * 3f;
        yield return new WaitForSeconds(0.1f);
        sr.color = originalColor;
    }
}