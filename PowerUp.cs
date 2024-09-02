using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public AudioClip coin;
    public AudioClip extraLife;
    public AudioClip magicpower;
    private AudioSource audioSource;

    public enum Type
    {
        Coin,
        ExtraLife,
        MagicMushroom,
        Starpower,
        Magicpower,
    }

    void Start()  // AUDIO
    {
        audioSource = GetComponent<AudioSource>();
    }

    void PlaySound(AudioClip clip)  // AUDIO
    {
        audioSource.clip = clip;
        audioSource.Play();
    }

    public Type type;

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
                PlaySound(coin);
                //MusicManager.Instance.PlayCoinSound();
                break;

            case Type.ExtraLife:
                GameManager.Instance.AddLife();
                PlaySound(extraLife);
                //MusicManager.Instance.PlayExtraLifeSound();
                break;

            case Type.MagicMushroom:
                player.GetComponent<Player>().Grow();
                break;

            case Type.Starpower:
                player.GetComponent<Player>().Starpower();
                break;

            case Type.Magicpower:
                player.GetComponent<Player>().Magicpower();
                PlaySound(magicpower);
                //MusicManager.Instance.PlayMagicPowerSound();
                break;
        }

        Destroy(gameObject);  // Kun peliobjekti on kerätty, se tuhotaan.
    }
}


