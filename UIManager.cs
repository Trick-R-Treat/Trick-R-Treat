using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text livesText;
    public Text coinsText;

    void Update()
    {
        if (GameManager.Instance != null)
        {
            livesText.text = "Lives: " + GameManager.Instance.lives;
            coinsText.text = "Coins: " + GameManager.Instance.coins;
        }
    }
}