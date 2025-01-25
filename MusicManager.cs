using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioSource backgroundMusic;
    public AudioSource gameOverMusic;
    public AudioSource mainMenuMusic;

    private static MusicManager instance = null;

    public static MusicManager Instance
    {
        get { return instance; }
    }

    // Awake -metodi varmistaa, ett‰ vain yksi instanssi BackgroundMusicManager-luokasta on olemassa
    // koko pelin ajan. Jos toinen instanssi yritet‰‰n luoda, se tuhotaan.
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
    }

    public void PlayBackgroundMusic(int world, int stage)
    {
        if (backgroundMusic != null)
        {
            backgroundMusic.Stop();  //Pys‰ytet‰‰n mahdollinen aiemmin soitettu musiikki.
        }

        backgroundMusic.Play();  //Soitetaan taustamusiikki
    }

    public void StopBackgroundMusic()
    {
        if (backgroundMusic != null)
        {
            backgroundMusic.Stop();
        }
    }

    public void PlayGameOverMusic()
    {
        if (backgroundMusic != null)
        {
            backgroundMusic.Stop();
        }
        //else
        //{
        //    Debug.LogWarning("backgroundMusic is null");
        //}

        if (gameOverMusic != null)
        {
            gameOverMusic.Play();
        }
        //else
        //{
        //    Debug.LogError("gameOverMusic is null");
        //}
    }

    public void PlayMainMenuMusic()
    {
        StopAllMusic(); //Pys‰ytt‰‰ kaiken musiikin ennen uuden aloittamista.
        
        if (mainMenuMusic != null)
        {
            mainMenuMusic.Play();
        }
    }

    private void StopAllMusic()
    {
        if (backgroundMusic != null) backgroundMusic.Stop();
        if (gameOverMusic != null) gameOverMusic.Stop();
        if (mainMenuMusic != null) mainMenuMusic.Stop();
    }
}
