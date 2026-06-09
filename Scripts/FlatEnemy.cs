using UnityEngine;

public class FlatEnemy : MonoBehaviour
{
    public Sprite flatSprite;
    public GameObject deathEffect;
    public AudioClip deathSound;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
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
                Flatten();
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

    private void Flatten()
    {
        GetComponent<Collider2D>().enabled = false;
        GetComponent<EntityMovement>().enabled = false;
        GetComponent<AnimatedSprite>().enabled = false;
        GetComponent<SpriteRenderer>().sprite = flatSprite;

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

    private void PlayDeathSound()
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
