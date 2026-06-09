using UnityEngine;

public class AngryEnemy : MonoBehaviour
{
    public AudioClip angrySound;
    public float angrySoundVolume = 1f;
    public AudioClip deathSound;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player player = collision.gameObject.GetComponent<Player>();

            if (player.starpower | player.magicpower)
            {
                Hit();
                GameManager.Instance.AddScore(10);
            }
            else
            {
                player.Hit();
                PlayAngrySound();
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

    private void Hit()
    {
        GetComponent<AnimatedSprite>().enabled = false;
        GetComponent<DeathAnimation>().enabled = true;

        PlayDeathSound();

        Destroy(gameObject, 3f);
    }

    private void PlayAngrySound()
    {
        if (angrySound != null)
        {
            GameObject soundObject = new GameObject("AngrySound");
            AudioSource tempAudio = soundObject.AddComponent<AudioSource>();

            tempAudio.clip = angrySound;
            tempAudio.volume = angrySoundVolume;
            tempAudio.Play();

            Destroy(soundObject, angrySound.length);
        }
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
}
