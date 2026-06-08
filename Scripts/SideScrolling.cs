using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideScrolling : MonoBehaviour
{
    private Transform player;

    [Header("Camera Heights")]
    public float height = 7f;
    public float undergroundHeight = -19f;

    private bool isUnderground = false;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    private void LateUpdate()
    {
        Vector3 cameraPosition = transform.position;
        //cameraPosition.x = Mathf.Max(cameraPosition.x, player.position.x);  //The camera only moves to the right with the player, like in the original game.
        cameraPosition.x = player.position.x;  //The camera moves left and right with the player.
        transform.position = cameraPosition;
    }

    public void SetUnderground(bool underground)
    {
        if (isUnderground == underground) return;

        isUnderground = underground;

        Vector3 cameraPosition = transform.position;
        cameraPosition.y = underground ? undergroundHeight : height;  
        transform.position = cameraPosition;

        if (MusicManager.Instance != null)
        {
            if (underground)
            {
                MusicManager.Instance.PlayUndergroundMusic();
            }
            else
            {
                MusicManager.Instance.PlayOvergroundMusic();
            }
        }
    }
}
