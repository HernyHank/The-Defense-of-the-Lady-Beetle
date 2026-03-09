using UnityEngine;

public class AsteroidFieldSpawner : MonoBehaviour
{
    [Header("Asteroid Prefabs")]
    public GameObject[] asteroidPrefabs;

    [Header("Field Settings")]
    public int asteroidCount = 100;

    public float spawnWidth = 40f;
    public float spawnHeight = 30f;

    public float spawnDistance = 80f; // how far in front of player field begins
    public float spawnDepth = 60f;    // thickness of the field along Z

    void Start()
    {
        SpawnField();
    }

    void SpawnField()
    {
        for (int i = 0; i < asteroidCount; i++)
        {
            GameObject prefab = asteroidPrefabs[Random.Range(0, asteroidPrefabs.Length)];

            float x = Random.Range(-spawnWidth, spawnWidth);
            float y = Random.Range(-spawnHeight, spawnHeight);

            // Spawn somewhere between distance and distance+depth
            float z = Random.Range(spawnDistance, spawnDistance + spawnDepth);

            Vector3 spawnPos = new Vector3(x, y, z);

            Quaternion rot = Random.rotation;

            GameObject asteroid = Instantiate(prefab, spawnPos, rot);

            asteroid.transform.parent = transform;

            // Optional: scale variation
            asteroid.transform.localScale *= Random.Range(0.7f, 1.6f);
        }
    }
}