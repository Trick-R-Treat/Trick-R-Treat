using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Broom : MonoBehaviour
{
    public AudioClip deathSound;  //AUDIO
    private AudioSource audioSource;  //AUDIO

    private void Start()
    {
        audioSource = GetComponent<AudioSource>(); //AUDIO
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))  //Jos Broom törmää pelaajan kanssa...
        {
            Player player = collision.gameObject.GetComponent<Player>();

            if (player.starpower | player.magicpower)  //Jos pelaajalla on tähti/taikavoima...
            {
                Hit();  //...Broom saa osuman.
                GameManager.Instance.AddScore(100);
            }
            else  //Muuten...
            {
                player.Hit();  //...pelaaja saa osuman.
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Shell"))  //Jos kuori osuu Broomiin...
        {
            Hit();  //...Broom saa osuman.
            GameManager.Instance.AddScore(100);
        }
    }

    private void Hit()
    {
        GetComponent<AnimatedSprite>().enabled = false;  //Poistetaan animaatiot käytöstä.
        GetComponent<DeathAnimation>().enabled = true;  //Toteutetaan kuoleman animaatio.

        PlayDeathSound(); //Soita kuolemaääni

        Destroy(gameObject, 3f);  //Poistetaan Broom näkyvistä 3 sekuntin kuluttua.
    }

    private void PlayDeathSound()
    {
        if (deathSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(deathSound);
        }
    }
}
