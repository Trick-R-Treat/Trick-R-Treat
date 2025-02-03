using UnityEngine;

public class Pumpkin : MonoBehaviour
{
    public Sprite flatSprite;
    public AudioClip deathSound;  //AUDIO
    private AudioSource audioSource;  //AUDIO

    private void Start()
    {
        audioSource = GetComponent<AudioSource>(); //AUDIO
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))  //Jos Pumpkin törmää pelaajan kanssa...
        {
            Player player = collision.gameObject.GetComponent<Player>();

            if (player.starpower | player.magicpower)  //Jos pelaajalla on tähti/taikavoima...
            {
                Hit();  //...Pumpkin saa osuman.
                GameManager.Instance.AddScore(100);
            }
            if (collision.transform.DotTest(transform, Vector2.down))  //Jos pelaaja hyppää Pumpkinin päälle...
            {
                Flatten();  //...Pumpkin litistyy.
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
        if (other.gameObject.layer == LayerMask.NameToLayer("Shell"))  //Jos kuori osuu...
        {
            Hit();  //Pumpkin saa osuman
            GameManager.Instance.AddScore(100);
        }

        if (other.CompareTag("Bullet")) //Jos ammus osuu...
        {
            Hit(); //Pumpkin saa osuman
            GameManager.Instance.AddScore(100);
            Destroy(other.gameObject); //Tuhoa ammus
        }
    }

    private void Flatten()
    {
        GetComponent<Collider2D>().enabled = false;  //Poistetaan törmäys käytöstä.
        GetComponent<EntityMovement>().enabled = false;  //Poistetaan liikkuminen käytöstä.
        GetComponent<AnimatedSprite>().enabled = false;  //Poistetaan animaatiot käytöstä.
        GetComponent<SpriteRenderer>().sprite = flatSprite;  //Haetaan hahmon litistetty muoto.

        PlayDeathSound();  //AUDIO

        Destroy(gameObject, 0.5f);  //Poistetaan Pumpkin näkyvistä puolen sekuntin kuluttua.
    }

    private void Hit()
    {
        GetComponent<AnimatedSprite>().enabled = false;  //Poistetaan animaatiot käytöstä.
        GetComponent<DeathAnimation>().enabled = true;  //Toteutetaan kuoleman animaatio.

        PlayDeathSound();  //AUDIO

        Destroy(gameObject, 3f);  //Poistetaan Pumpkin näkyvistä 3 sekuntin kuluttua.
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
