using UnityEngine;

public class DeathBarrier : MonoBehaviour
{
    public AudioClip death;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void PlaySound(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlaySound(death);
            other.gameObject.SetActive(false);

            GameManager.Instance.ResetLevel(3f);
        }

        else
        {
            Destroy(other.gameObject);
        }
    }
}
