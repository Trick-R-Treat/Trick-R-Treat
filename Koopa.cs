using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Koopa : MonoBehaviour
{
    public Sprite shellSprite;
    public float shellSpeed = 12f;  //Kuoren liikenopeus on t�ss� m��ritetty 12f.

    private bool shelled;
    private bool pushed;

    public AudioClip deathSound;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>(); //Hae AudioSource-komponentti
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!shelled && collision.gameObject.CompareTag("Player"))  //Jos Koopa ei ole kuoressaan ja t�rm�� pelaajan kanssa...
        {
            Player player = collision.gameObject.GetComponent<Player>();

            if (player.starpower | player.magicpower)  //Jos pelaajalla on t�htivoima...
            {
                Hit();  //...Koopa saa osuman.
            }
            else if (collision.transform.DotTest(transform, Vector2.down))  //Jos pelaaja hypp�� Koopan p��lle...
            {
                EnterShell();  //...Koopa menee kuoreensa sis�lle.
            }
            else  //Muuten...
            {
                player.Hit();  //...pelaaja saa osuman.
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (shelled && other.CompareTag("Player"))  //Jos Koopa on kuoressaan ja pelaaja t�rm�� siihen... 
        {
            if (!pushed)  //Jos kuorta ei ole ty�nnetty eli se ei liiku...
            {
                Vector2 direction = new Vector2(transform.position.x - other.transform.position.x, 0f);
                PushShell(direction);  //...kuori l�htee liikkumaan samaan suuntaan mihin sit� ty�nnettiin.
            }
            else  //Muuten...
            {
                Player player = other.GetComponent<Player>();
                
                if (player.starpower | player.magicpower)  //Jos pelaajalla on t�htivoima...
                {
                    Hit();  //...Koopa saa osuman.
                }
                else  //Muuten...
                {
                    player.Hit();  //...pelaaja saa osuman.
                }
            }
        }

        else if (!shelled && other.gameObject.layer == LayerMask.NameToLayer("Shell"))  // Jos Koopa ei ole kuoressaan ja toinen kuori osuu siihen...
        {
            Hit();  //...Koopa saa osuman.
        }
    }

    private void EnterShell()
    {
        shelled = true;

        GetComponent<EntityMovement>().enabled = false;  //Poistetaan liikkuminen k�yt�st�.
        GetComponent<AnimatedSprite>().enabled = false;  //Poistetaan animaatiot k�yt�st�.
        GetComponent<SpriteRenderer>().sprite = shellSprite;  //Haetaan hahmon kuorimuoto.
    }

    private void PushShell(Vector2 direction)
    {
        pushed = true;  //Kuorta on ty�nnetty.
        GetComponent<Rigidbody2D>().isKinematic = false;  //Otetaan Rigidbody uudelleen k�ytt��n ja poistetaan kinemaattisuus, jotta fysiikan moottori k�sittelee liikkeen.
        EntityMovement movement = GetComponent<EntityMovement>();  //Otetaan liikkuminen uudelleen k�ytt��n.
        movement.direction = direction.normalized;  //Kuori liikkuu samaan suuntaan mihin ty�nsimme sit�.
        movement.speed = shellSpeed;  //Nopeus on yht� suuri kuin kuoren nopeus.
        movement.enabled = true;

        //Unity -> Edit -> Project Settings -> Tags and Layers -> User Layers 8 kohtaan lis�tty "Shell"
        //Physics 2D -> Layer Collision Matrix kohdasta poistettu Shellin alin t�pp�.
        gameObject.layer = LayerMask.NameToLayer("Shell");
    }

    private void Hit()
    {
        GetComponent<AnimatedSprite>().enabled = false;  //Poistetaan animaatiot k�yt�st�.
        GetComponent<DeathAnimation>().enabled = true;  //Toteutetaan kuoleman animaatio.

        PlayDeathSound(); // Soita kuolema��ni

        Destroy(gameObject, 3f);  //Poistetaan Koopa n�kyvist� 3 sekuntin kuluttua.
    }

    private void PlayDeathSound()
    {
        if (deathSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(deathSound);
        }
    }

    //private void OnBecameInvisible()  //T�t� ei v�ltt�m�tt� tarvitse kirjoittaa koodiin!
    //{
    //    if (pushed)  //Jos kuorta on ty�nnetty...
    //    {
    //        Destroy(gameObject));  //...poista kohde n�kyvist� kun se on ruudun ulkopuolella.
    //    }
    //}
}