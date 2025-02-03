using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public Rigidbody2D rb;
    
    public int damage = 10;  //Ammuksen vahinko

    private Renderer bulletRenderer;

    private void Start()
    {
        rb.velocity = transform.right * speed;
        bulletRenderer = GetComponent<Renderer>();
    }

    private void OnTriggerEnter2D (Collider2D collision)
    {
        if (collision.tag == "Player")  //Jos ammus osuu pelaajaan, älä tee mitään
            return;

        if (collision.CompareTag("Boss")) //Tarkista, onko osuma Bossiin
        {
            BossHealth bossHealth = collision.GetComponent<BossHealth>();
            if (bossHealth != null)
            {
                bossHealth.TakeDamage(damage); //Anna vahinko Bossille
            }
        }

        //Tarkistetaan, onko ammus pelaajan kamerassa ennen vihollisen tappamista
        if (bulletRenderer != null && bulletRenderer.isVisible)
        {
            if (collision.CompareTag("Enemy")) //Tarkista, onko osuma viholliseen
            {
                Destroy(collision.gameObject); //Tuhoa vihollinen
            }

            Destroy(gameObject);
        }
        //Debug.Log(collision.name);
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject); //Tuhoa ammus, kun se poistuu kameran näkökentästä
    }
}
