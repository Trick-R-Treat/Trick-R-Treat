using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slimer : MonoBehaviour
{
    public AudioClip deathSound;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>(); //Hae AudioSource-komponentti
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))  //Jos Slimer t�rm�� pelaajan kanssa...
        {
            Player player = collision.gameObject.GetComponent<Player>();

            if (player.starpower | player.magicpower)  //Jos pelaajalla on t�htivoima...
            {
                Hit();  //...Slimer saa osuman.
            }
            else  //Muuten...
            {
                player.Hit();  //...pelaaja saa osuman.
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Shell"))  //Jos kuori osuu Slimeriin...
        {
            Hit();  //...Slimer saa osuman.
        }
    }

    private void Hit()
    {
        GetComponent<AnimatedSprite>().enabled = false;  //Poistetaan animaatiot k�yt�st�.
        GetComponent<DeathAnimation>().enabled = true;  //Toteutetaan kuoleman animaatio.

        PlayDeathSound(); // Soita kuolema��ni

        Destroy(gameObject, 3f);  //Poistetaan Slimer n�kyvist� 3 sekuntin kuluttua.
    }

    private void PlayDeathSound()
    {
        if (deathSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(deathSound);
        }
    }
}