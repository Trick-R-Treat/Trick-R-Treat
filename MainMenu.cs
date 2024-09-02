using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        // Soita musiikki, kun Main Menu latautuu
        if (audioSource != null)
        {
            audioSource.Play();
        }
    }

    public void PlayGame()
    {
        // Pysäytä musiikki, kun Play-nappia painetaan
        if (audioSource != null)
        {
            audioSource.Stop();
        }

        SceneManager.LoadSceneAsync(1);
    }

    public void QuitGame()
    {
        // Pysäytä musiikki, kun Quit-nappia painetaan
        if (audioSource != null)
        {
            audioSource.Stop();
        }

        Debug.Log("Quitting game...");
        Application.Quit();
    }
}


