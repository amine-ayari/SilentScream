using UnityEngine;
using UnityEngine.XR.ARFoundation;
using System.Collections.Generic;

public class ARObjectSpawner : MonoBehaviour
{
    public GameObject prefabToSpawn; // Assign your prefab in the Inspector
    public Transform player; // Assign your player in the Inspector
    private ARPlaneManager arPlaneManager;

    private int currentWave = 0; // Current wave number
    private float totalSurfaceArea; 
    private List<GameObject> spawnedEnemies = new List<GameObject>(); // List to track spawned enemies
    private int totalEnemiesToSpawn; // Total enemies to spawn for the current wave

    private void Awake()
    {
        arPlaneManager = GetComponent<ARPlaneManager>();
        totalSurfaceArea = 0f;
    }

    private void Update()
    {
        // Reset total surface area
        //totalSurfaceArea = 0f; 
        foreach (var plane in arPlaneManager.trackables)
        {
            if (plane.trackingState == UnityEngine.XR.ARSubsystems.TrackingState.Tracking)
            {
                totalSurfaceArea += CalculatePlaneArea(plane);
            }
        }

        // Log the total surface area
        Debug.Log("Total Surface Area: " + totalSurfaceArea);
        int timer = PlayerPrefs.GetInt("timer"); // Check PlayerPrefs variable
        
        // Check if it's time to spawn enemies based on waves
        if (timer == 1 && totalSurfaceArea >= 20f && spawnedEnemies.Count == 0)
        {
            SpawnWave();
            Debug.Log("Wave " + currentWave + " spawned!");
        }

        // Check if all enemies in the current wave are destroyed to move to the next wave
        CheckWaveCompletion();
    }

    private float CalculatePlaneArea(ARPlane plane)
    {
        Vector3 size = plane.extents; // Get the size of the plane
        float area = size.x * size.y; // Calculate the area (x * z dimensions)
        Debug.Log("Area: " + area);
        return area;
    }

    private void SpawnWave()
    {
        if (currentWave >= 3) return; // Only allow up to 3 waves

        currentWave++; // Increment the wave number
        totalEnemiesToSpawn = currentWave; // Number of enemies for the current wave

        for (int i = 0; i < totalEnemiesToSpawn; i++)
        {
            Vector3 spawnPosition = GetRandomSpawnPosition();
            GameObject enemy = Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);
            spawnedEnemies.Add(enemy); // Track the spawned enemy
        }
    }

    private Vector3 GetRandomSpawnPosition()
{
    float distance = 5f; // Distance in front of the player to spawn enemies
    float angle = Random.Range(-45f, 45f); // Random angle deviation from the player's forward direction

    // Calculate a direction vector that deviates from the player's forward direction by a random angle
    Vector3 direction = Quaternion.Euler(0, angle, 0) * player.forward;

    // Calculate the spawn position based on the direction and distance
    Vector3 spawnPosition = player.position + direction * distance;
    spawnPosition.y = -4; // Set the y position to -4

    return spawnPosition;
}


    private void CheckWaveCompletion()
    {
        // Remove destroyed enemies from the list
        spawnedEnemies.RemoveAll(enemy => enemy == null);

        // Check if all enemies in the current wave have been destroyed
        if (spawnedEnemies.Count == 0 && currentWave < 3)
        {
            Debug.Log("All enemies destroyed. Preparing for the next wave.");
        }
    }
}
