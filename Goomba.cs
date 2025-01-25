using UnityEngine;

public class Goomba : MonoBehaviour
{
    public Sprite flatSprite;
    public AudioClip deathSound;  //AUDIO
    private AudioSource audioSource;  //AUDIO

    private void Start()
    {
        audioSource = GetComponent<AudioSource>(); //AUDIO Hae AudioSource-komponentti
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))  //Jos Goomba törmää pelaajan kanssa...
        {
            Player player = collision.gameObject.GetComponent<Player>();

            if (player.starpower || player.magicpower)  //Jos pelaajalla on tähti/taikavoima...
            {
                Hit();  //...Goomba saa osuman.
                GameManager.Instance.AddScore(100);
            }
            else if (collision.transform.DotTest(transform, Vector2.down))  //Jos pelaaja hyppää Goomban päälle...
            {
                Flatten();  //...Goomba litistyy.
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
        if (other.gameObject.layer == LayerMask.NameToLayer("Shell"))  //Jos Koopan kuori osuu Goombaan...
        {
            Hit();  //...Goomba saa osuman.
        }

        if (other.CompareTag("Bullet")) //Jos ammus osuu...
        {
            Hit(); //Goomba saa osuman
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

        PlayDeathSound(); //AUDIO Soita kuolemaääni

        Destroy(gameObject, 0.5f);  //Poistetaan Goomba näkyvistä puolen sekuntin kuluttua.
    }

    private void Hit()
    {
        GetComponent<AnimatedSprite>().enabled = false;  //Poistetaan animaatiot käytöstä.
        GetComponent<DeathAnimation>().enabled = true;  //Toteutetaan kuoleman animaatio.

        PlayDeathSound(); //AUDIO Soita kuolemaääni

        Destroy(gameObject, 3f);  //Poistetaan Goomba näkyvistä 3 sekuntin kuluttua.
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
