using UnityEngine;

public class Pumpkin : MonoBehaviour
{
    public Sprite flatSprite;
    public AudioClip deathSound;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>(); //Hae AudioSource-komponentti
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))  //Jos vihollinen t�rm�� pelaajan kanssa...
        {
            Player player = collision.gameObject.GetComponent<Player>();

            if (player.starpower | player.magicpower)  //Jos pelaajalla on t�htivoima...
            {
                Hit();  //...vihollinen saa osuman.
            }
            else if (collision.transform.DotTest(transform, Vector2.down))  //Jos pelaaja hypp�� p��lle...
            {
                Flatten();  //...vihollinen litistyy.
            }
            else  //Muuten...
            {
                player.Hit();  //...pelaaja saa osuman.
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Shell"))  //Jos kuori osuu viholliseen...
        {
            Hit();  //...vihollinen saa osuman.
        }
    }

    private void Flatten()
    {
        GetComponent<Collider2D>().enabled = false;  //Poistetaan t�rm�ys k�yt�st�.
        GetComponent<EntityMovement>().enabled = false;  //Poistetaan liikkuminen k�yt�st�.
        GetComponent<AnimatedSprite>().enabled = false;  //Poistetaan animaatiot k�yt�st�.
        GetComponent<SpriteRenderer>().sprite = flatSprite;  //Haetaan hahmon litistetty muoto.

        PlayDeathSound(); // Soita kuolema��ni

        Destroy(gameObject, 0.5f);  //Poistetaan vihollinen n�kyvist� puolen sekuntin kuluttua.
    }

    private void Hit()
    {
        GetComponent<AnimatedSprite>().enabled = false;  //Poistetaan animaatiot k�yt�st�.
        GetComponent<DeathAnimation>().enabled = true;  //Toteutetaan kuoleman animaatio.

        PlayDeathSound(); // Soita kuolema��ni

        Destroy(gameObject, 3f);  //Poistetaan vihollinen n�kyvist� 3 sekuntin kuluttua.
    }

    private void PlayDeathSound()
    {
        if (deathSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(deathSound);
        }
    }
}