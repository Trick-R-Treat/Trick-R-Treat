using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public Rigidbody2D rb;
    public GameObject hitEffect;

    private Renderer bulletRenderer;

    public int damage = 10;

    private void Start()
    {
        rb.linearVelocity = transform.right * speed;
        bulletRenderer = GetComponent<Renderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            return;

        if (collision.CompareTag("Boss"))
        {
            BossHealth bossHealth = collision.GetComponent<BossHealth>();
            if (bossHealth != null)
            {
                bossHealth.TakeDamage(damage);
            }

            Debug.Log("Hit: " + collision.tag);
        }

        if (bulletRenderer != null && bulletRenderer.isVisible)
        {
            if (collision.CompareTag("Enemy"))
            {
                if (hitEffect != null)
                {
                    Instantiate(hitEffect, transform.position, Quaternion.identity);
                }
            }

            Debug.Log("Hit: " + collision.tag);
        }

        Destroy(gameObject);
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
