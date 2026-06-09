using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    public PlayerSpriteRenderer smallRenderer;
    public PlayerSpriteRenderer bigRenderer;
    private PlayerSpriteRenderer activeRenderer;

    private DeathAnimation deathAnimation;
    private CapsuleCollider2D capsuleCollider;

    public bool big => bigRenderer.enabled;
    public bool small => smallRenderer.enabled;
    public bool dead => deathAnimation.enabled;
    
    public bool starpower { get; private set; }
    public bool magicpower { get; private set; }

    private DeathBarrier deathBarrier;

    public GameObject bulletPrefab;
    public Transform shootingPoint;

    public AudioClip jump;
    public AudioClip death;
    public AudioClip powerUp;
    public AudioClip shrink;
    public AudioClip starAudio;
    public AudioClip magicPowerAudio;
    private AudioSource audioSource;
    private AudioSource starAudioSource;
    public AudioClip shootingSound;

    private void Awake()
    {
        deathAnimation = GetComponent<DeathAnimation>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        activeRenderer = smallRenderer;
        starAudioSource = gameObject.AddComponent<AudioSource>();
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        
        //Player can only shoot if it is big (Keyboard X button, Gamepad X button)
        //if (Input.GetKeyDown(KeyCode.X) && big)
            if ((Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.JoystickButton2)) && big)
        {
            Shoot();
            PlayShootingSound();
        }

        //if (Input.GetButtonDown("Jump"))
        if (Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.JoystickButton0))
        {
            PlaySound(jump);
        }

        //if (Input.GetKeyDown(KeyCode.JoystickButton2))
        //    Debug.Log("X");
    }

    void PlaySound(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }

    void PlayStarSound(AudioClip clip)
    {
        starAudioSource.clip = clip;
        starAudioSource.Play();
    }

    public void Hit()
    {
        if (!dead && !starpower && !magicpower)
        {
            if (big)
            {
                Shrink();
            }

            else
            {
                Death();
            }
        }
    }

    private void Death()
    {
        if (!dead)
        {
            smallRenderer.enabled = false;
            bigRenderer.enabled = false;
            deathAnimation.enabled = true;
            PlaySound(death);

            GameManager.Instance.ResetLevel(3f);
        }
    }

    public void Grow()
    {
        smallRenderer.enabled = false;
        bigRenderer.enabled = true;
        activeRenderer = bigRenderer;

        capsuleCollider.size = new Vector2(1f, 1.5f);
        capsuleCollider.offset = new Vector2(0f, 0.26f);

        StartCoroutine(ScaleAnimation());
        PlaySound(powerUp);
    }

    private void Shrink()
    {
        smallRenderer.enabled = true;
        bigRenderer.enabled = false;
        activeRenderer = smallRenderer;

        capsuleCollider.size = new Vector2(1f, 1f);
        capsuleCollider.offset = new Vector2(0f, 0f);

        StartCoroutine(ScaleAnimation());
        PlaySound(shrink);
    }

    private IEnumerator ScaleAnimation()
    {
        float elapsed = 0f;
        float duration = 0.5f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            if (Time.frameCount % 4 == 0)
            {
                smallRenderer.enabled = !smallRenderer.enabled;
                bigRenderer.enabled = !smallRenderer.enabled;
            }

            yield return null;
        }

        smallRenderer.enabled = false;
        bigRenderer.enabled = false;
        activeRenderer.enabled = true;
    }

    public void Starpower(float duration = 10f)
    {
        StartCoroutine(StarpowerCoroutine(duration));
        StartCoroutine(StarpowerAnimation(duration));
    }

    private IEnumerator StarpowerCoroutine(float duration)
    {
        PlayStarSound(starAudio);
        yield return new WaitForSeconds(duration);
        starAudioSource.Stop();
    }

    private IEnumerator StarpowerAnimation(float duration)
    {
        starpower = true;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            if (Time.frameCount % 4 == 0)
            {
                activeRenderer.spriteRenderer.color = Random.ColorHSV(0f, 1f, 1f, 1f, 1f, 1f);
            }

            yield return null;
        }

        activeRenderer.spriteRenderer.color = Color.white;
        starpower = false;
    }

    public void Magicpower(float duration = 1000f)
    {
        StartCoroutine(MagicpowerCoroutine(duration));
        StartCoroutine(MagicpowerAnimation(duration));
    }

    private IEnumerator MagicpowerCoroutine(float duration)
    {
        PlayStarSound(magicPowerAudio);
        yield return new WaitForSeconds(duration);
        starAudioSource.Stop();
    }

    private IEnumerator MagicpowerAnimation(float duration)
    {
        magicpower = true;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            if (Time.frameCount % 4 == 0)
            {
                activeRenderer.spriteRenderer.color = Random.ColorHSV(0f, 1f, 1f, 1f, 1f, 1f);
            }

            yield return null;
        }

        activeRenderer.spriteRenderer.color = Color.white;
        magicpower = false;
    }

    private void Shoot()
    {
        if (big)
        {
            Instantiate(bulletPrefab, shootingPoint.position, shootingPoint.rotation);
        }
    }

    private void PlayShootingSound()
    {
        if (big && shootingSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(shootingSound);
        }
    }
}
