using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    private Transform target;
    public float speed;
    public Sprite skullSprite;
    private new Rigidbody2D rigidbody;  //Lis‰‰m‰ll‰ sanan "new" rigidbodyn eteen, saa poistettua virheilmoituksen.
    SpriteRenderer SR;
    private Vector2 velocity;

    public AudioClip deathSound;  //AUDIO
    private AudioSource audioSource;  //AUDIO

    private void Awake()
    {
        SR = GetComponent<SpriteRenderer>();
        rigidbody = GetComponent<Rigidbody2D>();
        enabled = false;  //Objektien ei haluta liikkuvan heti, vaan ainoastaan sitten kun ne aktivoituvat.
    }

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        audioSource = GetComponent<AudioSource>(); //AUDIO
    }

    void Update()
    {
        //Liikutetaan Ghostia pelaajaa kohti
        transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        //Selvitet‰‰n liikesuunta
        Vector2 direction = target.position - transform.position;

        //K‰‰nnet‰‰n sprite oikeaan suuntaan
        if (direction.x > 0) //Liike oikealle
        {
            transform.localScale = new Vector3(1, 1, 1); //Oikea suunta
        }
        else if (direction.x < 0) //Liike vasemmalle
        {
            transform.localScale = new Vector3(-1, 1, 1); //K‰‰nn‰ sprite horisontaalisesti
        }
    }

    private void OnBecameVisible()  //Objektit aktivoituvat kun ne n‰kyv‰t n‰ytˆll‰.
    {
        enabled = true;
    }

    private void OnBecameInvisible()  //Objektit eiv‰t ole aktiivisia kun ne eiv‰t n‰y n‰ytˆll‰.
    {
        enabled = false;
    }

    private void OnEnable()  //Objekti liikkuu kun se on aktivoitu.
    {
        rigidbody.WakeUp();
    }

    private void OnDisable()  //Objekti ei liiku kun se ei ole aktiivinen.
    {
        rigidbody.velocity = Vector2.zero;  //Objektin liike pys‰ytet‰‰n.
        rigidbody.Sleep();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))  //Jos Ghost tˆrm‰‰ pelaajan kanssa...
        {
            Player player = collision.gameObject.GetComponent<Player>();

            if (player.starpower || player.magicpower)  //Jos pelaajalla on t‰hti/taikavoima...
            {
                Hit();  //...Ghost saa osuman.
                GameManager.Instance.AddScore(100);
            }
            else if (collision.transform.DotTest(transform, Vector2.down))  //Jos pelaaja hypp‰‰ Ghostin p‰‰lle...
            {
                Skull();  //...Ghost muuttaa muotoa ja katoaa.
                GameManager.Instance.AddScore(100);
            }
            else  //Muuten...
            {
                player.Hit();  //...pelaaja saa osuman.
                Destroy(gameObject, 0.2f);  //Poistetaan Ghost n‰kyvist‰ 0.2 sekuntin kuluttua.
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Shell"))  //Jos kuori osuu Ghostiin...
        {
            Hit();  //Ghost saa osuman
        }

        if (other.CompareTag("Bullet")) //Jos ammus osuu...
        {
            Hit(); //Ghost saa osuman
            GameManager.Instance.AddScore(100);
            Destroy(other.gameObject); //Tuhoa ammus
        }
    }

    private void Skull()
    {
        //Poistetaan tˆrm‰ykset ja liikkuminen
        Collider2D[] colliders = GetComponents<Collider2D>();
        foreach (Collider2D collider in colliders)
        {
            collider.enabled = false;
        }
        
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.zero; //Pys‰ytet‰‰n liike
            rb.isKinematic = true; //Estet‰‰n fysiikka
        }

        //Poistetaan animaatiot ja liikkeen hallinta
        EntityMovement entityMovement = GetComponent<EntityMovement>();
        if (entityMovement != null)
        {
            entityMovement.enabled = false;
        }
        
        AnimatedSprite animatedSprite = GetComponent<AnimatedSprite>();
        if (animatedSprite != null)
        {
            animatedSprite.enabled = false;
        }

        //P‰ivitet‰‰n Ghostin ulkoasu p‰‰kalloksi
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null && skullSprite != null)
        {
            spriteRenderer.sprite = skullSprite;
        }

        PlayDeathSound(); //AUDIO Soita kuolema‰‰ni

        Destroy(gameObject, 0.5f);  //Poistetaan Ghost n‰kyvist‰ puolen sekuntin kuluttua.
    }

    private void Hit()
    {
        GetComponent<AnimatedSprite>().enabled = false;  //Poistetaan animaatiot k‰ytˆst‰.
        GetComponent<DeathAnimation>().enabled = true;  //Toteutetaan kuoleman animaatio.

        PlayDeathSound(); //AUDIO Soita kuolema‰‰ni

        Destroy(gameObject, 3f);  //Poistetaan Ghost n‰kyvist‰ 3 sekuntin kuluttua.
    }

    public void PlayDeathSound()  //AUDIO
    {
        if (deathSound != null)
        {
            GameObject soundObject = new GameObject("DeathSound");
            AudioSource tempAudio = soundObject.AddComponent<AudioSource>();
            tempAudio.clip = deathSound;
            tempAudio.Play();
            Destroy(soundObject, deathSound.length); //Tuhoa ‰‰niobjekti, kun ‰‰ni on toistettu
        }

        //if (deathSound != null && audioSource != null)
        //{
        //    audioSource.PlayOneShot(deathSound);
        //}
    }
}
