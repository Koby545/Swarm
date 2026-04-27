using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 6;
    private int currentHealth;
    public float damageCooldown = 1f;
    private float lastDamageTime = -1f;
    private SpriteRenderer sr;
    [HideInInspector] public bool damageReduction = false;

    public Sprite heartSprite;
    public Sprite halfHeartSprite;
    public Transform heartsContainer;
    private List<Image> heartImages = new List<Image>();



    void Start()
    {
        currentHealth = maxHealth;
        sr = GetComponent<SpriteRenderer>();
        UpdateHearts();
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            if (Time.time - lastDamageTime >= damageCooldown)
            {
                lastDamageTime = Time.time;

                int damage = 2; // ברירת מחדל – זומבי רגיל = לב שלם

                if (other.gameObject.name.Contains("Fast"))
                    damage = 1; // חצי לב
                else if (other.gameObject.name.Contains("Big"))
                    damage = 3; // לב וחצי
                else if (other.gameObject.GetComponent<Boss>() != null)
                    damage = 4; // שני לבבות

                // Fast enemy עושה חצי לב = 1 נזק כל 2 פגיעות
                if (other.gameObject.name.Contains("Fast"))
                    damage = 1;

                TakeDamage(damage);

                Rigidbody2D enemyRb = other.gameObject.GetComponent<Rigidbody2D>();
                if (enemyRb != null)
                {
                    Vector2 pushDir = (other.transform.position - transform.position).normalized;
                    enemyRb.AddForce(pushDir * 5f, ForceMode2D.Impulse);
                }
            }
        }
    }

    void TakeDamage(int damage)
    {
        if (damageReduction) damage = Mathf.Max(1, damage - 1);

        currentHealth -= damage;
        AudioManager.Instance.PlayHurt();
        StartCoroutine(CameraShake.Instance.Shake(0.2f, 0.3f));
        StartCoroutine(FlashRed());
        UpdateHearts();

        if (currentHealth <= 0)
        {
            GameManager.Instance.GameOver();
            Destroy(gameObject);
        }
    }

    void UpdateHearts()
    {
        GameManager.Instance.UpdateHealth(currentHealth);

        foreach (var h in heartImages)
            Destroy(h.gameObject);
        heartImages.Clear();

        int fullHearts = currentHealth / 2;
        bool halfHeart = currentHealth % 2 == 1;
        int total = fullHearts + (halfHeart ? 1 : 0);
        float heartSize = 50f;
        float spacing = 55f;
        int heartsPerRow = 5;

        for (int i = 0; i < fullHearts; i++)
        {
            int row = i / heartsPerRow;
            int col = i % heartsPerRow;

            GameObject heartObj = new GameObject("Heart");
            heartObj.transform.SetParent(heartsContainer);
            Image img = heartObj.AddComponent<Image>();
            img.sprite = heartSprite;
            img.rectTransform.sizeDelta = new Vector2(heartSize, heartSize);
            img.rectTransform.anchoredPosition = new Vector2(col * spacing, -row * spacing);
            img.rectTransform.localScale = Vector3.one;
            heartImages.Add(img);
        }

        if (halfHeart)
        {
            int row = fullHearts / heartsPerRow;
            int col = fullHearts % heartsPerRow;

            GameObject heartObj = new GameObject("HalfHeart");
            heartObj.transform.SetParent(heartsContainer);
            Image img = heartObj.AddComponent<Image>();
            img.sprite = halfHeartSprite != null ? halfHeartSprite : heartSprite;
            img.rectTransform.sizeDelta = new Vector2(heartSize, heartSize);
            img.rectTransform.anchoredPosition = new Vector2(col * spacing, -row * spacing);
            img.rectTransform.localScale = Vector3.one;
            if (halfHeartSprite == null) img.color = new Color(1, 1, 1, 0.5f);
            heartImages.Add(img);
        }
    }

    IEnumerator FlashRed()
    {
        sr.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        sr.color = Color.white;
        yield return new WaitForSeconds(0.1f);
        sr.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        sr.color = Color.white;
    }

    public void Heal(int amount)
    {
        currentHealth = currentHealth + amount;
        UpdateHearts();
    }
}