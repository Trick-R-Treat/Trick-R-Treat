using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int world { get; private set; }
    public int stage { get; private set; }
    public int lives { get; private set; }
    public int coins { get; private set; }
    public int score { get; private set; }

    public AudioClip coinSound;
    public AudioClip extraLifeSound;
    private AudioSource audioSource;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    private void Start()
    {
        //The frame rate is set to 60FPS.
        //Application.targetFrameRate = 60;
        //{
        audioSource = GetComponent<AudioSource>();
        NewGame();
        //}
    }

    public void NewGame()
    {
        world = 1;
        stage = 1;
        lives = 3;
        coins = 0;
        score = 0;

        LoadLevel(1, 1);
    }

    public void LoadLevel(int world, int stage)
    {
        this.world = world;
        this.stage = stage;

        SceneManager.LoadScene($"{world}-{stage}");
        MusicManager.Instance.PlayBackgroundMusic(world, stage);
    }

    public void NextLevel()
    {
        if (world == 1 && stage == 2)
        {
            LoadLevel(world + 1, 1);
        }
    }

    public void LevelComplete()
    {
        MusicManager.Instance.StopBackgroundMusic();

        if (MusicManager.Instance != null)
        {
            MusicManager.Instance.PlayLevelCompleteMusic();
        }
    }

    public void ResetLevel(float delay)
    {
        Invoke(nameof(ResetLevel), delay);
    }

    public void ResetLevel()
    {
        lives--;

        if (lives > 0)
        {
            LoadLevel(world, stage);
        }
        else
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        //NewGame();  //Game Over restarts the game.
        //Invoke(nameof(NewGame), 3f);  //After the game ends, a new game will be started after 3 seconds.

        SceneManager.LoadScene("GameOver");
        
        if (MusicManager.Instance != null)
        {
            MusicManager.Instance.PlayGameOverMusic();
        }

    }

    public void AddCoin()
    {
        coins++;

        if (coinSound != null)
            audioSource.PlayOneShot(coinSound, 1.0f);

        if (coins == 100)
        {
            AddLife();
            coins = 0;
        }
    }

    public void AddLife()
    {
        lives++;

        if (extraLifeSound != null)
            audioSource.PlayOneShot(extraLifeSound, 1.0f);
    }

    public void AddScore(int points)
    {
        score += points;
        //Debug.Log($"Added {points} points. Total score: {score}");
        //FindAnyObjectByType<UIManager>().UpdateScore(score);

        var ui = FindAnyObjectByType<UIManager>();
        if (ui != null)
        {
            ui.UpdateScore(score);
        }
    }

    public void LoadEndScene()
    {
        Debug.Log("Loading Credits scene...");

        SceneManager.LoadScene("Credits");

        if (MusicManager.Instance != null)
        {
            MusicManager.Instance.StopBackgroundMusic();
        }
    }
}
