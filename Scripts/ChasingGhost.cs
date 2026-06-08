using UnityEngine;

public class ChasingGhost : MonoBehaviour
{
    private Transform target;
    public float speed;
    public Sprite skullSprite;
    private new Rigidbody2D rigidbody;
    SpriteRenderer SR;
    private Vector3 originalScale;
    public GameObject deathEffect;

    public AudioClip deathSound;
    private AudioSource audioSource;

    private void Awake()
    {
        SR = GetComponent<SpriteRenderer>();
        rigidbody = GetComponent<Rigidbody2D>();
        originalScale = transform.localScale;
        enabled = false;
    }

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        //GROUND-MOVING ENEMY > Move towards the player (only in the X direction)
        //float newX = Mathf.MoveTowards(transform.position.x, target.position.x, speed * Time.deltaTime);
        //transform.position = new Vector2(newX, transform.position.y);

        //FLYING ENEMY > Move towards the player
        transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        Vector2 direction = target.position - transform.position;

        if (direction.x > 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
        }
        else if (direction.x < 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
        }
    }

    private void OnBecameVisible()
    {
        enabled = true;
    }

    private void OnBecameInvisible()
    {
        enabled = false;
    }

    private void OnEnable()
    {
        rigidbody.WakeUp();
    }

    private void OnDisable()
    {
        rigidbody.linearVelocity = Vector2.zero;
        rigidbody.Sleep();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player player = collision.gameObject.GetComponent<Player>();

            if (player.starpower || player.magicpower)
            {
                Hit();
                GameManager.Instance.AddScore(10);
            }
            else if (collision.transform.DotTest(transform, Vector2.down))
            {
                Skull();
                GameManager.Instance.AddScore(100);
            }
            else
            {
                player.Hit();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Shell"))
        {
            Hit();
            GameManager.Instance.AddScore(100);
        }

        if (other.CompareTag("Bullet"))
        {
            Hit();
            GameManager.Instance.AddScore(10);
            Destroy(other.gameObject);
        }
    }

    private void Skull()
    {
        Collider2D[] colliders = GetComponents<Collider2D>();
        foreach (Collider2D collider in colliders)
        {
            collider.enabled = false;
        }

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Kinematic;
        }

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

        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null && skullSprite != null)
        {
            spriteRenderer.sprite = skullSprite;
        }

        PlayDeathSound();

        SpawnDeathEffect();

        Destroy(gameObject, 0.5f);
    }

    private void Hit()
    {
        GetComponent<AnimatedSprite>().enabled = false;
        GetComponent<DeathAnimation>().enabled = true;

        PlayDeathSound();

        Destroy(gameObject, 3f);
    }

    public void PlayDeathSound()
    {
        if (deathSound != null)
        {
            GameObject soundObject = new GameObject("DeathSound");
            AudioSource tempAudio = soundObject.AddComponent<AudioSource>();
            tempAudio.clip = deathSound;
            tempAudio.Play();
            Destroy(soundObject, deathSound.length);
        }
    }

    private void SpawnDeathEffect()
    {
        if (deathEffect != null)
        {
            Instantiate(deathEffect, transform.position, Quaternion.identity);
        }
    }
}
