using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnailRocket : MonoBehaviour
{
    public AudioClip deathSound;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>(); // Hae AudioSource-komponentti
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))  // Jos etana törmää pelaajan kanssa...
        {
            Player player = collision.gameObject.GetComponent<Player>();

            if (player.starpower | player.magicpower)  // Jos pelaajalla on tähtivoima...
            {
                Hit();  //...etana saa osuman.
            }
            else if (collision.transform.DotTest(transform, Vector2.down))  // Jos pelaaja hyppää etanan päälle...
            {
                Hit();  //...etana saa osuman.
            }
            else  // Muuten...
            {
                player.Hit();  //...pelaaja saa osuman.
            }
        }
    }

    private void Hit()
    {
        GetComponent<AnimatedSprite>().enabled = false;  // Poistetaan animaatiot käytöstä.
        GetComponent<DeathAnimation>().enabled = true;  // Toteutetaan kuoleman animaatio.

        PlayDeathSound(); // Soita kuolemaääni

        Destroy(gameObject, 3f);  // Poistetaan etana näkyvistä 3 sekuntin kuluttua.
    }

    private void PlayDeathSound()
    {
        if (deathSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(deathSound);
        }
    }
}