using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AsteroidFieldSpawner : MonoBehaviour
{
    public EventController controller;
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
        //SpawnField(asteroidCount);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Keypad8) || Input.GetKeyDown(KeyCode.L))
        {
            ClearField();
        }
    }
    public void SpawnField(int numberOfAsteroids, float asteroidDuration)
    {
        for (int i = 0; i < numberOfAsteroids; i++)
        {
            GameObject prefab = asteroidPrefabs[Random.Range(0, asteroidPrefabs.Length)];

            
            float y = Random.Range(-spawnHeight, spawnHeight);
            float z = Random.Range(-spawnWidth, spawnWidth);
            // Spawn somewhere between distance and distance+depth

            float x = Random.Range(spawnDistance, spawnDistance + spawnDepth);

            Vector3 spawnPos = new Vector3(x, y, z);

            Quaternion rot = Random.rotation;

            GameObject asteroid = Instantiate(prefab, spawnPos, rot);

            asteroid.transform.parent = transform;

            // Optional: scale variation
            asteroid.transform.localScale *= Random.Range(0.7f, 1.6f);
        }
        StartCoroutine(KeepAsteroids(asteroidDuration));
    }

    IEnumerator KeepAsteroids(float duration)
    {
        yield return new WaitForSeconds(duration);
        ClearField();
    }

    public void ClearField()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
                Destroy(transform.GetChild(i).gameObject);
                controller.steroidsSpawned = false;
        }

    }
}