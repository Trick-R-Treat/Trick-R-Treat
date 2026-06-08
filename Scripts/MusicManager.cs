using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    [Header("Level Music")]
    public AudioSource world1Stage1Music;
    public AudioSource world1Stage2Music;
    public AudioSource world2Stage1Music;

    [Header("Special Music")]
    public AudioSource undergroundMusic;
    public AudioSource levelCompleteMusic;
    public AudioSource mainMenuMusic;
    public AudioSource gameOverMusic;
    public AudioSource creditsMusic;

    private float musicVolume = 1f;

    private static MusicManager instance = null;

    public static MusicManager Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }

        DontDestroyOnLoad(this.gameObject);

        musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        SetMusicVolume(musicVolume);
    }

    private AudioSource GetLevelMusic(int world, int stage)
    {
        if (world == 1 && stage == 1)
            return world1Stage1Music;

        if (world == 1 && stage == 2)
            return world1Stage2Music;

        if (world == 2 && stage == 1)
            return world2Stage1Music;

        return null;
    }

    public void PlayBackgroundMusic(int world, int stage)
    {
        StopAllMusic();

        AudioSource music = GetLevelMusic(world, stage);

        if (music != null)
        {
            music.Play();
        }
    }

    public void StopBackgroundMusic()
    {
        StopAllMusic();
    }

    public void PlayUndergroundMusic()
    {
        StopAllMusic();

        if (undergroundMusic != null)
        {
            undergroundMusic.Play();
        }
    }

    public void PlayOvergroundMusic()
    {
        if (GameManager.Instance == null)
            return;

        PlayBackgroundMusic(
            GameManager.Instance.world,
            GameManager.Instance.stage
        );
    }

    public void PlayLevelCompleteMusic()
    {
        StopAllMusic();

        if (levelCompleteMusic != null)
        {
            levelCompleteMusic.Play();
        }
    }

    public void PlayMainMenuMusic()
    {
        StopAllMusic();

        if (mainMenuMusic != null)
        {
            mainMenuMusic.Play();
        }
    }

    public void PlayGameOverMusic()
    {
        StopAllMusic();

        if (gameOverMusic != null)
        {
            gameOverMusic.Play();
        }
    }

    public void PlayCreditsMusic()
    {
        StopAllMusic();

        if (creditsMusic != null)
        {
            creditsMusic.Play();
        }
    }

    private void StopAllMusic()
    {
        if (world1Stage1Music != null) world1Stage1Music.Stop();
        if (world1Stage2Music != null) world1Stage2Music.Stop();
        if (world2Stage1Music != null) world2Stage1Music.Stop();
        if (undergroundMusic != null) undergroundMusic.Stop();
        if (levelCompleteMusic != null) levelCompleteMusic.Stop();
        if (mainMenuMusic != null) mainMenuMusic.Stop();
        if (gameOverMusic != null) gameOverMusic.Stop();
        if (creditsMusic != null) creditsMusic.Stop();
    }

    public void SetMusicVolume(float volume)
    {
        if (world1Stage1Music != null) world1Stage1Music.volume = volume;
        if (world1Stage2Music != null) world1Stage2Music.volume = volume;
        if (world2Stage1Music != null) world2Stage1Music.volume = volume;

        if (undergroundMusic != null) undergroundMusic.volume = volume;
        if (levelCompleteMusic != null) levelCompleteMusic.volume = volume;
        if (mainMenuMusic != null) mainMenuMusic.volume = volume;
        if (gameOverMusic != null) gameOverMusic.volume = volume;
        if (creditsMusic != null) creditsMusic.volume = volume;
    }
}
