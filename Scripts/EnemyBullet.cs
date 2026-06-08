using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public enum BulletType { Fire, Ice }
    public BulletType bulletType;

    public float speed = 5f;
    private Vector2 direction;

    public AudioClip shootSound;
    private AudioSource audioSource;

    public void SetDirection(Vector2 dir)
    {
        direction = dir;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        if (audioSource != null && shootSound != null)
        {
            audioSource.PlayOneShot(shootSound);
        }
    }

    void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        //audioSource.volume = 1f;  //Volume
        //audioSource.spatialBlend = 0f; //2D audio (not location-aware)
    }

    void Update()
    {
        transform.position += (Vector3)direction * speed * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
            PlayerMovement movement = collision.GetComponent<PlayerMovement>();

            if (player != null)
            {
                if (player.starpower || player.magicpower)
                {
                    Destroy(gameObject);
                    return;
                }

                if (bulletType == BulletType.Fire)
                {
                    player?.Hit();
                }
                else if (bulletType == BulletType.Ice)
                {
                    movement?.Freeze(2f);
                }
            }

            Destroy(gameObject);
        }
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
