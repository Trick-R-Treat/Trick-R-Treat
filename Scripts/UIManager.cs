using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text livesText;
    public Text coinsText;
    public float timeLeft = 300;  //T‰ss‰ on m‰‰ritetty, ett‰ pelaajalla on tason suorittamiseen aikaa 300 sekunttia.
    public int playerScore = 0;
    public GameObject timeUI;
    public GameObject scoreUI;

    private Player player;

    void Update()
    {
        if (GameManager.Instance != null)
        {
            livesText.text = "Lives: " + GameManager.Instance.lives;
            coinsText.text = "Coins: " + GameManager.Instance.coins;

            //Debug.Log(timeLeft);
            timeLeft -= Time.deltaTime;

            if (timeUI != null)
            { 
                timeUI.gameObject.GetComponent<Text>().text = ("Time: " + (int)timeLeft);
            }

            if (scoreUI != null)
            {
                scoreUI.gameObject.GetComponent<Text>().text = ("Score: " + GameManager.Instance.score);  //Pelin pisteet p‰ivittyv‰t t‰‰lt‰.
            }

            if (timeLeft < 0.1f)  //Jos aika loppuu resetoidaan taso.
            {
                GameManager.Instance.ResetLevel(0f);
            }

            //if (Input.GetKeyDown(KeyCode.Space))
            //{
            //    GameManager.Instance.AddScore(50);
            //}
        }
    }

    public void UpdateScore(int newScore)
    {
        playerScore = newScore;
        //Debug.Log($"Updating score to: {playerScore}");
        
        if (scoreUI != null)
        {
            scoreUI.gameObject.GetComponent<Text>().text = "Score: " + playerScore;
        }
    }

    void CountScore()
    {
        playerScore = playerScore + (int)(timeLeft * 10);
        //Debug.Log(playerScore);
    }

    public void ResetTime()
    {
        timeLeft = 300;
    }
}
