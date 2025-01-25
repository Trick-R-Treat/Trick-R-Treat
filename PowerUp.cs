using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public Type type;

    public enum Type
    {
        Coin,
        ExtraLife,
        MagicMushroom,
        Starpower,
        Magicpower,
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Collect(other.gameObject);
        }
    }

    private void Collect(GameObject player)
    {
        switch (type)
        {
            case Type.Coin:
                GameManager.Instance.AddCoin();
                GameManager.Instance.AddScore(10);
                break;

            case Type.ExtraLife:
                GameManager.Instance.AddLife();
                GameManager.Instance.AddScore(10);
                break;

            case Type.MagicMushroom:
                player.GetComponent<Player>().Grow();
                GameManager.Instance.AddScore(10);
                break;

            case Type.Starpower:
                player.GetComponent<Player>().Starpower();
                GameManager.Instance.AddScore(10);
                break;

            case Type.Magicpower:
                player.GetComponent<Player>().Magicpower();
                GameManager.Instance.AddScore(10);
                break;
        }

        Destroy(gameObject);  //Kun peliobjekti on kerätty, se tuhotaan.
    }
}
