using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Bettle : MonoBehaviour
{
    public AudioClip deathSound;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>(); // Hae AudioSource-komponentti
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))  // Jos Bettle törmää pelaajan kanssa...
        {
            Player player = collision.gameObject.GetComponent<Player>();

            if (player.starpower | player.magicpower)  // Jos pelaajalla on tähtivoima...
            {
                Hit();  // ...Bettle saa osuman.
            }
            else  // Muuten...
            {
                player.Hit();  // ...pelaaja saa osuman.
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Shell"))  // Jos kuori osuu Bettleen...
        {
            Hit();  // ...Bettle saa osuman.
        }
    }

    private void Hit()
    {
        GetComponent<AnimatedSprite>().enabled = false;  // Poistetaan animaatiot käytöstä.
        GetComponent<DeathAnimation>().enabled = true;  // Toteutetaan kuoleman animaatio.

        PlayDeathSound(); // Soita kuolemaääni

        Destroy(gameObject, 3f);  // Poistetaan Bettle näkyvistä 3 sekuntin kuluttua.
    }

     private void PlayDeathSound()
     {
        if (deathSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(deathSound);
        }
     }
}
