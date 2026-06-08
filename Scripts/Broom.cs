using UnityEngine;

public class Broom : MonoBehaviour
{
    public float angryDuration = 20f;
    public float angrySpeed = 3f;
    public float normalSpeed = 1f;
    public float speedRampTime = 1.5f;
    public float shakeAmount = 0.05f;
    private Vector3 originalPosition;

    private bool isAngry = false;
    private EntityMovement movement;

    public AudioClip deathSound;
    private AudioSource audioSource; 

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        movement = GetComponent<EntityMovement>();
    }

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
            if (!isAngry)
            {
                FaceBulletDirection(other.transform);
                GoAngry();
            }

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

    private void PlayDeathSound()
    {
        if (deathSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(deathSound);
        }
    }

    private void GoAngry()
    {
        if (isAngry) return;

        isAngry = true;
        StartCoroutine(AngryRoutine());
    }

    private System.Collections.IEnumerator AngryRoutine()
    {
        float shakeTime = 1f;
        float elapsed = 0f;

        originalPosition = transform.position;

        while (elapsed < speedRampTime)
        {
            elapsed += Time.deltaTime;

            float t = elapsed / speedRampTime;
            movement.speed = Mathf.Lerp(normalSpeed, angrySpeed, t);

            if (elapsed < shakeTime)
            {
                transform.position = originalPosition + (Vector3)Random.insideUnitCircle * shakeAmount;
            }
            else
            {
                transform.position = originalPosition;
            }

            yield return null;
        }

        transform.position = originalPosition;

        movement.speed = angrySpeed;

        yield return new WaitForSeconds(angryDuration);

        movement.speed = normalSpeed;
        isAngry = false;
    }

    private void FaceBulletDirection(Transform bullet)
    {
        if (bullet.position.x > transform.position.x)
        {
            movement.direction = Vector2.right;
            movement.GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            movement.direction = Vector2.left;
            movement.GetComponent<SpriteRenderer>().flipX = false;
        }
    }
}
