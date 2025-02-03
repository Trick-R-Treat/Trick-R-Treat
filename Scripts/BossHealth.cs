using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossHealth : MonoBehaviour
{
    public int health = 1000;
    public bool isInvulnerable = false;
    public GameObject deathEffect;
    private bool isDead = false; //Est‰‰ kuoleman logiikan kutsumisen useasti

    public GameObject healthBarUI; //Healthbar UI viittaus
    public UnityEngine.UI.Slider healthSlider; //Slider komponentti
    
    private Animator animator;
    
    public GameObject player; // Viittaus pelaajaan
    private Collider2D bossCollider;
    private Collider2D playerCollider;

    public AudioClip deathSound;  //AUDIO
    private AudioSource audioSource;  //AUDIO

    private void Start()
    {
        animator = GetComponent<Animator>();
        healthSlider.maxValue = health;
        healthSlider.value = health;
        audioSource = GetComponent<AudioSource>(); //AUDIO

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

        if (health <= 500)
        {
            animator.SetTrigger("Stunned");
        }

        if (health <= 0)
        {
            Die();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet")) //Jos ammus osuu
        {
            Bullet bullet = collision.GetComponent<Bullet>();
            
            if (bullet != null)
            {
                TakeDamage(bullet.damage); //V‰hennet‰‰n ammuksen vahinko
                GameManager.Instance.AddScore(10);
            }

            Destroy(collision.gameObject); //Tuhoa ammus
        }
    }
    void Die()
    {
        if (isDead) return;

        isDead = true; //Merkit‰‰n boss kuolleeksi
        animator.SetTrigger("Die"); //Aktivoi kuolema-animaatio
        healthBarUI.SetActive(false); //Piilota healthbar
        StartCoroutine(HandleDeath());

        PlayDeathSound(); //AUDIO Soita kuolema‰‰ni
        GameManager.Instance.AddScore(1000);

        if (playerCollider != null && bossCollider != null)
        {
            Physics2D.IgnoreCollision(bossCollider, playerCollider, true);
        }
    }

    IEnumerator HandleDeath()
    {
        // Odota kuolema-animaation keston ajan
        yield return new WaitForSeconds(10f); //S‰‰d‰ odotusaikaa animaation pituuden mukaan
        Instantiate(deathEffect, transform.position, Quaternion.identity); //Lis‰‰ visuaalinen efekti
        Destroy(gameObject); //Tuhoa boss

        GameManager.Instance.LoadEndScene();  //Lataa lopputekstit skene
    }

    private void PlayDeathSound()  //AUDIO
    {
        if (deathSound != null)
        {
            GameObject soundObject = new GameObject("DeathSound");
            AudioSource tempAudio = soundObject.AddComponent<AudioSource>();
            tempAudio.clip = deathSound;
            tempAudio.Play();
            Destroy(soundObject, deathSound.length); //Tuhoa ‰‰niobjekti, kun ‰‰ni on toistettu
        }
    }
}
