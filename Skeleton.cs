using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : MonoBehaviour
{
    public Sprite skullSprite;
    public AudioClip deathSound;  //AUDIO
    private AudioSource audioSource;  //AUDIO

    private void Start()
    {
        audioSource = GetComponent<AudioSource>(); //AUDIO Hae AudioSource-komponentti
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))  //Jos Skeleton törmää pelaajan kanssa...
        {
            Player player = collision.gameObject.GetComponent<Player>();

            if (player.starpower || player.magicpower)  //Jos pelaajalla on tähti/taikavoima...
            {
                Hit();  //...Skeleton saa osuman.
                GameManager.Instance.AddScore(100);
            }
            else if (collision.transform.DotTest(transform, Vector2.down))  //Jos pelaaja hyppää Skeletonin päälle...
            {
                Skull();  //...Skeleton saa osuman.
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
        if (other.gameObject.layer == LayerMask.NameToLayer("Shell"))  //Jos kuori osuu Skeletoniin...
        {
            Hit();  //...Skeleton saa osuman.
            GameManager.Instance.AddScore(100);
        }

        if (other.CompareTag("Bullet")) //Jos ammus osuu...
        {
            Hit(); //Skeleton saa osuman
            GameManager.Instance.AddScore(100);
            Destroy(other.gameObject); //Tuhoa ammus
        }
    }

    private void Skull()
    {
        GetComponent<Collider2D>().enabled = false;  //Poistetaan törmäys käytöstä.
        GetComponent<EnemyPatrol>().enabled = false;  //Poistetaan liikkuminen käytöstä.
        GetComponent<AnimatedSprite>().enabled = false;  //Poistetaan animaatiot käytöstä.
        GetComponent<SpriteRenderer>().sprite = skullSprite;  //Haetaan pääkallo muoto.

        PlayDeathSound(); //AUDIO Soita kuolemaääni

        Destroy(gameObject, 0.5f);  //Poistetaan Skeleton näkyvistä puolen sekuntin kuluttua.
    }

    private void Hit()
    {
        GetComponent<AnimatedSprite>().enabled = false;  //Poistetaan animaatiot käytöstä.
        GetComponent<DeathAnimation>().enabled = true;  //Toteutetaan kuoleman animaatio.

        PlayDeathSound(); //AUDIO Soita kuolemaääni

        Destroy(gameObject, 3f);  //Poistetaan Skeleton näkyvistä 3 sekuntin kuluttua.
    }

    public void PlayDeathSound()  //AUDIO
    {
        if (deathSound != null)
        {
            GameObject soundObject = new GameObject("DeathSound");
            AudioSource tempAudio = soundObject.AddComponent<AudioSource>();
            tempAudio.clip = deathSound;
            tempAudio.Play();
            Destroy(soundObject, deathSound.length); //Tuhoa ääniobjekti, kun ääni on toistettu
        }

        //if (deathSound != null && audioSource != null)
        //{
        //    audioSource.PlayOneShot(deathSound);
        //}
    }
}
