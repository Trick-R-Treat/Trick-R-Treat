using UnityEngine;

public class DeathBarrier : MonoBehaviour
{
    public AudioClip death;
    private AudioSource audioSource;

    void Start()  // AUDIO
    {
        audioSource = GetComponent<AudioSource>();
    }

    void PlaySound(AudioClip clip)  // AUDIO
    {
        audioSource.clip = clip;
        audioSource.Play();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))  // Jos pelaaja koskettaa kuoleman rajaa...
        {
           PlaySound(death);
           other.gameObject.SetActive(false);  // ...pelaaja asetetaan ei-aktiiviseksi.
           
           GameManager.Instance.ResetLevel(3f);  // Taso resetoidaan 3 sekuntin kuluttua.
        }

        else  // Jos joku muu objekti koskettaa kuoleman rajaa...
        {
            Destroy(other.gameObject);  // ...ne tuhotaan.
        }
    }
}
