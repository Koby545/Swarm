using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 15f;
    public int damage = 1;
    private Rigidbody2D rb;
    public bool explosive = false;
    public float explosionRadius = 1.5f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = transform.right * speed;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            if (explosive) Explode();
            Destroy(gameObject);
            return;
        }

        Boss boss = other.GetComponent<Boss>();
        if (boss != null)
        {
            boss.TakeDamage(damage);
            if (explosive) Explode();
            Destroy(gameObject);
        }
    }
    void Explode()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        foreach (var hit in hits)
        {
            Enemy enemy = hit.GetComponent<Enemy>();
            if (enemy != null) enemy.TakeDamage(damage);

            Boss boss = hit.GetComponent<Boss>();
            if (boss != null) boss.TakeDamage(damage);
        }
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}