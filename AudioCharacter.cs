using UnityEngine;

public class AudioCharacter : MonoBehaviour
{
    public AudioClip jump;     // Äänitiedosto hypylle
    public AudioClip death;    // Äänitiedosto kuolemalle
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
            PlaySound(jump);  // Soita ääni kun pelaaja hyppää
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
        PlaySound(death);  // Soita ääni kun pelaaja kuolee
    }

    void PlaySound(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }
}