using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


public class BossHealth : MonoBehaviour
{
    public int health = 1000;
    public bool isInvulnerable = false;
    public GameObject deathEffect;
    private GameObject spawnedDeathEffect;
    private bool isDead = false;
    public bool IsDead => isDead;

    public GameObject healthBarUI;
    public UnityEngine.UI.Slider healthSlider;

    private Animator animator;

    public GameObject player;
    private Collider2D bossCollider;
    private Collider2D playerCollider;

    public AudioClip deathSound;
    private AudioSource audioSource;

    public GameObject despawnEffect;

    private void Start()
    {
        animator = GetComponent<Animator>();
        healthSlider.maxValue = health;
        healthSlider.value = health;
        audioSource = GetComponent<AudioSource>();

        bossCollider = GetComponent<Collider2D>();
        
        if (player != null)
        {
            playerCollider = player.GetComponent<Collider2D>();
        }
    }

    public void TakeDamage(int damage)
    {
        if (isInvulnerable || isDead) return;

        health -= damage;
        healthSlider.value = health;

        if (health <= 500 && !isDead)
        {
            animator.SetTrigger("Stunned");
        }

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Boss Die() kutsuttu");

        if (isDead) return; 

        isDead = true;

        Debug.Log("Aloitetaan spawnattujen objektien tuhoaminen");

        ItemSpawner.StopAllSpawning = true;

        DestroyAllSpawnedObjects();

        Debug.Log("Spawnatut objektit tuhottu");

        // Make the player immortal
        //Player playerScript = player.GetComponent<Player>();
        //if (playerScript != null)
        //{
        //    playerScript.Magicpower();
        //}

        animator.SetTrigger("Die");

        // Pysäytä liike täysin
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Static;
        rb.simulated = false;

        spawnedDeathEffect = Instantiate(deathEffect, transform.position, Quaternion.identity);

        healthBarUI.SetActive(false);

        if (UIManager.Instance != null)
        {
            UIManager.Instance.StopTime();
        }

        StartCoroutine(HandleDeath());

        PlayDeathSound();
        GameManager.Instance.AddScore(10000);

        if (playerCollider != null && bossCollider != null)
        {
            Physics2D.IgnoreCollision(bossCollider, playerCollider, true);
        }

        StartCoroutine(NotifyLevelComplete());
    }

    IEnumerator NotifyLevelComplete()
    {
        yield return new WaitForSecondsRealtime(4f);

        GameManager.Instance.LevelComplete();
    }

    IEnumerator HandleDeath()
    {
        Debug.Log("Coroutine started");

        yield return new WaitForSecondsRealtime(10f);

        Debug.Log("After wait");

        if (spawnedDeathEffect != null)
        {
            Destroy(spawnedDeathEffect);
        }

        StartCoroutine(LoadCreditsNextFrame());
    }

    IEnumerator LoadCreditsNextFrame()
    {
        Debug.Log("Before scene load");

        yield return null;

        GameManager.Instance.LoadEndScene();

        Debug.Log("After scene load");
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

    private void DestroyAllSpawnedObjects()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in enemies)
        {
            if (enemy == null)
                continue;

            if (enemy == gameObject)
                continue;

            if (despawnEffect != null)
            {
                GameObject effect = Instantiate(despawnEffect,enemy.transform.position,Quaternion.identity);

                Destroy(effect, 2f);
            }

            Destroy(enemy);
        }

        GameObject[] powerUps = GameObject.FindGameObjectsWithTag("PowerUp");

        foreach (GameObject powerUp in powerUps)
        {
            if (powerUp == null)
                continue;

            if (despawnEffect != null)
            {
                GameObject effect = Instantiate(despawnEffect,powerUp.transform.position,Quaternion.identity);

                Destroy(effect, 2f);
            }

            Destroy(powerUp);
        }
    }
}
