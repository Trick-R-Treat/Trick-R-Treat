using UnityEngine;

[System.Serializable]
public class WeightedItem
{
    public GameObject prefab;
    public int weight;
}

public class ItemSpawner : MonoBehaviour
{
    public static bool StopAllSpawning = false;

    [SerializeField]
    private WeightedItem[] items;

    [SerializeField]
    private float minimumSpawnTime;

    [SerializeField]
    private float maximumSpawnTime;

    private float timeUntilSpawn;

    private bool isVisible = false;

    void Awake()
    {
        SetTimeUntilSpawn();
    }

    private void Update()
    {
        if (StopAllSpawning)
            return;

        if (FindAnyObjectByType<BossHealth>()?.IsDead == true)
            return;

        if (!isVisible) return;

        timeUntilSpawn -= Time.deltaTime;

        if (timeUntilSpawn <= 0)
        {
            GameObject itemToSpawn = GetRandomItem();

            if (itemToSpawn != null)
            {
                Instantiate(
                    itemToSpawn,
                    transform.position,
                    Quaternion.identity
                );
            }

            SetTimeUntilSpawn();
        }
    }

    private void SetTimeUntilSpawn()
    {
        timeUntilSpawn = Random.Range(minimumSpawnTime, maximumSpawnTime);
    }

    private GameObject GetRandomItem()
    {
        int totalWeight = 0;

        foreach (WeightedItem item in items)
        {
            totalWeight += item.weight;
        }

        if (totalWeight <= 0)
        {
            Debug.LogWarning("ItemSpawner: Kaikkien painojen summa on 0!");
            return null;
        }

        int randomValue = Random.Range(0, totalWeight);

        int currentWeight = 0;

        foreach (WeightedItem item in items)
        {
            currentWeight += item.weight;

            if (randomValue < currentWeight)
            {
                return item.prefab;
            }
        }

        return items[items.Length - 1].prefab;
    }

    private void OnBecameVisible()
    {
        isVisible = true;
    }

    private void OnBecameInvisible()
    {
        isVisible = false;
    }
}
