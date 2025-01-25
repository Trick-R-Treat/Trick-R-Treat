using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public Rigidbody2D rb;
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

        //Tarkistetaan, onko ammus pelaajan kamerassa ennen vihollisen tappamista
        if (bulletRenderer != null && bulletRenderer.isVisible)
        {
            if (collision.CompareTag("Enemy")) //Tarkista, onko osuma viholliseen
            {
                Destroy(collision.gameObject); //Tuhoa vihollinen
            }
        }

        //Debug.Log(collision.name);

        Destroy(gameObject);
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject); //Tuhoa ammus, kun se poistuu kameran näkökentästä
    }
}
