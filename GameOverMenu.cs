using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    public void RestartGame()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.NewGame();  //Kutsutaan GameManagerin NewGame-metodia                                 
            SceneManager.LoadScene("1-1");  //Ladataan ensimmäinen taso
        }
        else
        {
            Debug.LogError("GameManager instance is null. Make sure GameManager is present in the scene.");
        }
    }

    public void QuitToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}


