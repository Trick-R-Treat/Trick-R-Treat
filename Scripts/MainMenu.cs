using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (audioSource != null )
        {
            audioSource.Play();
        }

        MusicManager.Instance?.PlayMainMenuMusic();
    }

    public void PlayGame()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.NewGame();
        }

        if (audioSource != null)
        {
            audioSource.Stop();
        }

        SceneManager.LoadSceneAsync(1);
    }

    public void QuitGame()
    {
        if (audioSource != null)
        {
            audioSource.Stop();
        }

        Debug.Log("Quitting game...");
        Application.Quit();
    }
}
