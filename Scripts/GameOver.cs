using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public void GoToMainMenu()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.NewGame();
        }

        MusicManager.Instance?.PlayMainMenuMusic();
        
        SceneManager.LoadScene("MainMenu");
    }
}
