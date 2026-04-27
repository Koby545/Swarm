using UnityEngine;

public class DeathEffect : MonoBehaviour
{
    public int particleCount = 8;
    public float force = 3f;
    public Color color = Color.red;

    public void Explode()
    {
        for (int i = 0; i < particleCount; i++)
        {
            GameObject particle = new GameObject("Particle");
            particle.transform.position = transform.position;

            SpriteRenderer sr = particle.AddComponent<SpriteRenderer>();
            sr.sprite = GetComponent<SpriteRenderer>().sprite;
            sr.color = color;

            Rigidbody2D rb = particle.AddComponent<Rigidbody2D>();
            rb.gravityScale = 0f;

            Vector2 dir = Random.insideUnitCircle.normalized;
            rb.linearVelocity = dir * force;

            particle.transform.localScale = Vector3.one * Random.Range(0.1f, 0.25f);
            Destroy(particle, 0.4f);
        }
    }
}