using UnityEngine;

public class AudioCharacter : MonoBehaviour
{
    public AudioClip jump;     // Äänitiedosto hypylle
    public AudioClip death;
    public AudioClip powerUp;  // Äänitiedosto taikasienelle
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            PlaySound(jump);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("powerUp"))
        {
            PlaySound(powerUp);
            Destroy(other.gameObject); // Esineen tuhoaminen
        }
    }
    public void Death()
    {
        PlaySound(death);
        // Pelaajan kuolemisen logiikkaa, kuten animaatio tai pelin resetointi
        // Esimerkiksi: gameObject.SetActive(false); tai SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void PlaySound(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }
}