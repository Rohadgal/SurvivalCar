using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Spawns enemies at specified intervals from a pool at random spawn points.
/// </summary>
public class Enemy_Spawner : MonoBehaviour {
    public ObjectEnemyPool objectPool; // Reference to the ObjectPool for enemies
    public float spawnInterval = 2f; // Interval between enemy spawns
    public List<Transform> spawnPoints; // List of spawn points where enemies can spawn

    private float timer; // Timer to track spawn intervals

    void Start() {
        timer = spawnInterval; // Initialize timer
    }

    void Update() {
        timer -= Time.deltaTime; // Countdown the timer based on real time
        if (timer <= 0f) {
            GameObject enemy = objectPool.GetObjectFromPool(); // Get an enemy object from the pool
            if (enemy != null) {
                // Get a random spawn point from the list
                Transform randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
                enemy.transform.position = randomSpawnPoint.position; // Set enemy's position to the chosen spawn point
                // Reset health
            }
            timer = spawnInterval; // Reset the timer
        }
    }
}
