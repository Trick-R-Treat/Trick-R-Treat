using UnityEngine;
using TMPro;
using System.Collections;

public class UIManager : MonoBehaviour
{
    public TMP_Text scoreText;
    public TMP_Text coinsText;
    public TMP_Text livesText;
    public TMP_Text timeText;
    public TMP_Text timeUpText;

    public float timeLeft = 300f;
    public int playerScore = 0;

    private Color defaultTimeColor;
    private bool timeStopped = false;
    private bool timeUpTriggered = false;

    [Header("Warning Sound")]
    public AudioClip timeWarningClip;
    private AudioSource audioSource;
    private bool warningPlayed = false;

    [Header("Time Up Sound")]
    public AudioClip timeUpClip;

    public static UIManager Instance;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        defaultTimeColor = timeText.color;

        if (timeUpText != null)
            timeUpText.gameObject.SetActive(false);

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        warningPlayed = false;
    }

    void Update()
    {
        if (GameManager.Instance == null) return;

        livesText.text = "Lives: " + GameManager.Instance.lives;
        coinsText.text = "Coins: " + GameManager.Instance.coins;

        if (!timeStopped)
        {
            timeLeft -= Time.deltaTime;
            timeText.text = "Time: " + (int)timeLeft;
        }

        if (!timeStopped && timeLeft <= 60f && !warningPlayed)
        {
            audioSource.PlayOneShot(timeWarningClip);
            warningPlayed = true;
        }

        if (!timeStopped && timeLeft <= 60f)
        {
            //Text is flashing
            timeText.color = Color.Lerp(Color.white, Color.red,
                Mathf.PingPong(Time.time * 2f, 1));

            //Does not blink
            // timeText.color = Color.red;
        }
        else
        {
            timeText.color = defaultTimeColor;
        }

        if (!timeStopped && !timeUpTriggered && timeLeft <= 0f)
        {
            timeUpTriggered = true;
            StartCoroutine(TimeUpSequence());
        }

        scoreText.text = "Score: " + GameManager.Instance.score;
    }

    public void UpdateScore(int newScore)
    {
        playerScore = newScore;
        scoreText.text = "Score: " + playerScore;
        //Debug.Log($"Updating score to: {playerScore}");
    }

    public void ResetTime()
    {
        timeLeft = 300f;
        warningPlayed = false;
    }

    public void StopTime()
    {
        timeStopped = true;
    }

    private IEnumerator TimeUpSequence()
    {
        timeStopped = true;

        if (timeUpText != null)
            timeUpText.gameObject.SetActive(true);

        timeText.color = Color.red;

        if (MusicManager.Instance != null)
        {
            MusicManager.Instance.StopBackgroundMusic();
        }

        if (timeUpClip != null)
        {
            audioSource.PlayOneShot(timeUpClip);
        }

        Time.timeScale = 0f;

        yield return new WaitForSecondsRealtime(5f);

        Time.timeScale = 1f;

        GameManager.Instance.ResetLevel();
    }
}
