using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyPrefab;

    [SerializeField]
    private float minimumSpawnTime;

    [SerializeField]
    private float maximumSpawnTime;

    private float timeUntilSpawn;
    private bool isVisible = false;  //Tieto siit‰, onko spawn-paikka n‰kyviss‰.

    void Awake()
    {
        SetTimeUntilSpawn();
    }

    private void Update()
    {
        if (!isVisible) return;  //Ei tehd‰ mit‰‰n, jos spawn-paikka ei ole n‰kyviss‰.

        timeUntilSpawn -= Time.deltaTime;

        if (timeUntilSpawn <= 0)
        {
            Instantiate(enemyPrefab, transform.position, Quaternion.identity);
            SetTimeUntilSpawn();
        }
    }

    private void SetTimeUntilSpawn()
    {
        timeUntilSpawn = Random.Range(minimumSpawnTime, maximumSpawnTime);
    }

    private void OnBecameVisible()
    {
        isVisible = true;  //Spawn-paikka on n‰kyviss‰.
    }

    private void OnBecameInvisible()
    {
        isVisible = false;  //Spawn-paikka ei ole n‰kyviss‰.
    }
}
