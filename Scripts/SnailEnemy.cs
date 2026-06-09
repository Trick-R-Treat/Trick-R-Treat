using UnityEngine;

public class SnailEnemy : MonoBehaviour
{
    public Sprite shellSprite;
    public float shellSpeed = 12f;

    private bool shelled;
    private bool pushed;

    public AudioClip angrySound;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!shelled && collision.gameObject.CompareTag("Player"))
        {
            Player player = collision.gameObject.GetComponent<Player>();

            if (player.starpower | player.magicpower)
            {
                Hit();
                GameManager.Instance.AddScore(10);
            }
            else if (collision.transform.DotTest(transform, Vector2.down))
            {
                EnterShell();
            }
            else
            {
                player.Hit();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (shelled && other.CompareTag("Player"))
        {
            if (!pushed)
            {
                Vector2 direction = new Vector2(transform.position.x - other.transform.position.x, 0f);
                PushShell(direction); 
            }
            else
            {
                Player player = other.GetComponent<Player>();

                if (player.starpower | player.magicpower)
                {
                    Hit();
                    GameManager.Instance.AddScore(10);
                }
                else
                {
                    player.Hit();
                }
            }
        }

        else if (!shelled && other.gameObject.layer == LayerMask.NameToLayer("Shell"))
        {
            Hit();
            GameManager.Instance.AddScore(100);
        }

        if (!shelled && other.CompareTag("Bullet"))
        {
            EnterShell();
            PlayAngrySound();
            Destroy(other.gameObject);
        }
    }

    private void EnterShell()
    {
        shelled = true;

        GetComponent<EntityMovement>().enabled = false;
        GetComponent<AnimatedSprite>().enabled = false;
        GetComponent<SpriteRenderer>().sprite = shellSprite;
    }

    private void PushShell(Vector2 direction)
    {
        pushed = true;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        EntityMovement movement = GetComponent<EntityMovement>();
        movement.direction = direction.normalized;
        movement.speed = shellSpeed;
        movement.enabled = true;

        gameObject.layer = LayerMask.NameToLayer("Shell");
    }

    private void Hit()
    {
        GetComponent<AnimatedSprite>().enabled = false;
        GetComponent<DeathAnimation>().enabled = true;

        PlayAngrySound();

        Destroy(gameObject, 3f);
    }

    private void PlayAngrySound()
    {
        if (angrySound != null)
        {
            GameObject soundObject = new GameObject("AngrySound");
            AudioSource tempAudio = soundObject.AddComponent<AudioSource>();
            tempAudio.clip = angrySound;
            tempAudio.Play();
            Destroy(soundObject, angrySound.length);
        }
    }
}
