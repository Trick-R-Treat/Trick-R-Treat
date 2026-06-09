using UnityEngine;
using System.Collections;

public class FlagPole : MonoBehaviour
{
    public Transform flag;
    public Transform poleBottom;
    public Transform castle;
    public ParticleSystem fireworks;
    public float speed = 6f;

    public int nextWorld = 1;
    public int nextStage = 1;

    public AudioClip flagPole;
    public AudioClip fireWorks;
    private AudioSource audioSource;

    private bool levelCompleted = false;

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
        if (levelCompleted) return;

        if (other.CompareTag("Player"))
        {
            levelCompleted = true;
            Object.FindAnyObjectByType<UIManager>()?.StopTime();

            MusicManager.Instance?.PlayLevelCompleteMusic();

            StartCoroutine(MoveTo(flag, poleBottom.position));
            StartCoroutine(LevelCompleteSequence(other.transform));
            PlaySound(flagPole);
        }
    }

    private IEnumerator LevelCompleteSequence(Transform player)
    {
        player.GetComponent<PlayerMovement>().enabled = false;

        yield return MoveTo(player, poleBottom.position);
        yield return MoveTo(player, player.position + Vector3.right);
        yield return MoveTo(player, player.position + Vector3.right + Vector3.down);
        yield return MoveTo(player, castle.position);

        fireworks.Play();
        PlaySound(fireWorks);

        GameManager.Instance.AddScore(1000);

        player.gameObject.SetActive(false);

        yield return new WaitForSeconds(8f);

        GameManager.Instance.LoadLevel(nextWorld, nextStage);

        fireworks.Stop();
    }

    private IEnumerator MoveTo(Transform subject, Vector3 destination)
    {
        while (Vector3.Distance(subject.position, destination) > 0.128f)
        {
            subject.position = Vector3.MoveTowards(subject.position, destination, speed * Time.deltaTime);
            yield return null;
        }

        subject.position = destination;
    }
}
