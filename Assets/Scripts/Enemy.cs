using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public float speed = 2f;
    public int health = 3;
    public int scoreValue = 10;
    private Transform player;
    private float wobbleTimer;
    public float wobbleSpeed = 5f;
    public float wobbleAmount = 15f;
    private SpriteRenderer sr;
    private Color originalColor;

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
        transform.rotation = Quaternion.Euler(0, 0, angle + wobble);
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
            return;
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