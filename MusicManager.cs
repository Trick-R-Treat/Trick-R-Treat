using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioSource backgroundMusic;
    public AudioSource gameOverMusic;
    public AudioClip level3Music;
    //public AudioSource coinSound;
    //public AudioSource extraLifeSound;
    //public AudioSource magicPowerSound;

    private static MusicManager instance = null;

    public static MusicManager Instance
    {
        get { return instance; }
    }

    //Awake -metodi varmistaa, ett‰ vain yksi instanssi BackgroundMusicManager-luokasta on olemassa
    //koko pelin ajan. Jos toinen instanssi yritet‰‰n luoda, se tuhotaan.
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
            backgroundMusic.Stop();  //Pys‰ytet‰‰n mahdollinen aiemmin soitettu musiikki

            if (world == 1 && stage == 3 && level3Music != null)  // Jos ollaan tasolla 3
            {
                backgroundMusic.clip = level3Music;
            }
            else
            {
                // Asetetaan oletus taustamusiikki
                // Aseta t‰m‰ `AudioClip` Unity Inspectorissa `backgroundMusic.clip` kent‰ss‰
            }

            backgroundMusic.Play();  // Soitetaan valittu taustamusiikki
        }
        else
        {
            Debug.LogWarning("backgroundMusic is not set in the inspector.");
        }

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
        else
        {
            Debug.LogWarning("backgroundMusic is null");
        }

        if (gameOverMusic != null)
        {
            gameOverMusic.Play();
        }
        else
        {
            Debug.LogError("gameOverMusic is null");
        }
    }
}

