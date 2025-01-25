using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Koopa : MonoBehaviour
{
    public Sprite shellSprite;
    public float shellSpeed = 12f;  //Kuoren liikenopeus on tässä määritetty 12f.

    private bool shelled;
    private bool pushed;

    public AudioClip angrySound;  //AUDIO
    private AudioSource audioSource;  //AUDIO

    private void Start()
    {
        audioSource = GetComponent<AudioSource>(); //AUDIO
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!shelled && collision.gameObject.CompareTag("Player"))  //Jos Koopa ei ole kuoressaan ja törmää pelaajan kanssa...
        {
            Player player = collision.gameObject.GetComponent<Player>();

            if (player.starpower | player.magicpower)  //Jos pelaajalla on tähti/taikavoima...
            {
                Hit();  //...Koopa saa osuman.
                GameManager.Instance.AddScore(100);
            }
            else if (collision.transform.DotTest(transform, Vector2.down))  //Jos pelaaja hyppää Koopan päälle...
            {
                EnterShell();  //...Koopa menee kuoreensa sisälle.
            }
            else  //Muuten...
            {
                player.Hit();  //...pelaaja saa osuman.
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (shelled && other.CompareTag("Player"))  //Jos Koopa on kuoressaan ja pelaaja törmää siihen... 
        {
            if (!pushed)  //Jos kuorta ei ole työnnetty eli se ei liiku...
            {
                Vector2 direction = new Vector2(transform.position.x - other.transform.position.x, 0f);
                PushShell(direction);  //...kuori lähtee liikkumaan samaan suuntaan mihin sitä työnnettiin.
            }
            else  //Muuten...
            {
                Player player = other.GetComponent<Player>();

                if (player.starpower || player.magicpower)  //Jos pelaajalla on tähti/taikavoima...
                {
                    Hit();  //...Koopa saa osuman.
                    GameManager.Instance.AddScore(100);
                }
                else  //Muuten...
                {
                    player.Hit();  //...pelaaja saa osuman.
                }
            }
        }

        else if (!shelled && other.gameObject.layer == LayerMask.NameToLayer("Shell"))  //Jos Koopa ei ole kuoressaan ja toinen kuori osuu siihen...
        {
            Hit();  //...Koopa saa osuman.
            PlayAngrySound(); //AUDIO
        }
        
        //LISÄTTY
        if (!shelled && other.CompareTag("Bullet")) //Jos Koopa ei ole kuoressaan ja ammus osuu siihen...
        {
            EnterShell(); //Koopa menee kuoreen sisälle
            PlayAngrySound(); //AUDIO
            Destroy(other.gameObject); //Tuhoa ammus
        }
    }

private void EnterShell()
    {
        shelled = true;

        GetComponent<EntityMovement>().enabled = false;  //Poistetaan liikkuminen käytöstä.
        GetComponent<AnimatedSprite>().enabled = false;  //Poistetaan animaatiot käytöstä.
        GetComponent<SpriteRenderer>().sprite = shellSprite;  //Haetaan hahmon kuorimuoto.
    }

    private void PushShell(Vector2 direction)
    {
        pushed = true;  //Kuorta on työnnetty.
        GetComponent<Rigidbody2D>().isKinematic = false;  //Otetaan Rigidbody uudelleen käyttöön ja poistetaan kinemaattisuus, jotta fysiikan moottori käsittelee liikkeen.
        EntityMovement movement = GetComponent<EntityMovement>();  //Otetaan liikkuminen uudelleen käyttöön.
        movement.direction = direction.normalized;  //Kuori liikkuu samaan suuntaan mihin työnsimme sitä.
        movement.speed = shellSpeed;  //Nopeus on yhtä suuri kuin kuoren nopeus.
        movement.enabled = true;

        //Unity -> Edit -> Project Settings -> Tags and Layers -> User Layers 8 kohtaan lisätty "Shell"
        //Physics 2D -> Layer Collision Matrix kohdasta poistettu Shellin alin täppä.
        gameObject.layer = LayerMask.NameToLayer("Shell");
    }

    private void Hit()
    {
        GetComponent<AnimatedSprite>().enabled = false;  //Poistetaan animaatiot käytöstä.
        GetComponent<DeathAnimation>().enabled = true;  //Toteutetaan kuoleman animaatio.

        PlayAngrySound(); //AUDIO

        Destroy(gameObject, 3f);  //Poistetaan Koopa näkyvistä 3 sekuntin kuluttua.
    }

    private void PlayAngrySound()  //AUDIO
    {
        if (angrySound != null)
        {
            GameObject soundObject = new GameObject("AngrySound");
            AudioSource tempAudio = soundObject.AddComponent<AudioSource>();
            tempAudio.clip = angrySound;
            tempAudio.Play();
            Destroy(soundObject, angrySound.length); //Tuhoa ääniobjekti, kun ääni on toistettu
        }
    }

    //private void OnBecameInvisible()  //Tätä ei välttämättä tarvitse kirjoittaa koodiin!
    //{
    //    if (pushed)  //Jos kuorta on työnnetty...
    //    {
    //        Destroy(gameObject));  //...poista kohde näkyvistä kun se on ruudun ulkopuolella. Tällöin kuori katoaa pelistä kokonaan.
    //    }
    //}
}
