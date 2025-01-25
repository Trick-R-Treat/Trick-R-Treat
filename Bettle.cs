using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bettle : MonoBehaviour
{
    public AudioClip deathSound;  //AUDIO
    private AudioSource audioSource;  //AUDIO

    private void Start()
    {
        audioSource = GetComponent<AudioSource>(); //AUDIO
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))  //Jos Bettle törmää pelaajan kanssa...
        {
            Player player = collision.gameObject.GetComponent<Player>();

            if (player.starpower | player.magicpower)  //Jos pelaajalla on tähti/taikavoima...
            {
                Hit();  //...Bettle saa osuman.
            }
            else  //Muuten...
            {
                player.Hit();  //...pelaaja saa osuman.
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Shell"))  //Jos kuori osuu Bettleen...
        {
            Hit();  //...Bettle saa osuman.
            GameManager.Instance.AddScore(100);
        }

        if (other.CompareTag("Bullet")) //Jos ammus osuu...
        {
            Hit(); //Bettle saa osuman
            GameManager.Instance.AddScore(100);
            Destroy(other.gameObject); //Tuhoa ammus
        }
    }

    private void Hit()
    {
        GetComponent<AnimatedSprite>().enabled = false;  //Poistetaan animaatiot käytöstä.
        GetComponent<DeathAnimation>().enabled = true;  //Toteutetaan kuoleman animaatio.

        PlayDeathSound(); //Soita kuolemaääni

        Destroy(gameObject, 3f);  //Poistetaan Bettle näkyvistä 3 sekuntin kuluttua.
    }

    private void PlayDeathSound()  //AUDIO
    {
        if (deathSound != null)
        {
            GameObject soundObject = new GameObject("DeathSound");
            AudioSource tempAudio = soundObject.AddComponent<AudioSource>();
            tempAudio.clip = deathSound;
            tempAudio.Play();
            Destroy(soundObject, deathSound.length); //Tuhoa ääniobjekti, kun ääni on toistettu
        }
    }
}
