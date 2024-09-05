using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages a pool of enemy objects to optimize instantiation and destruction.
/// </summary>
public class ObjectEnemyPool : MonoBehaviour {
    public static ObjectEnemyPool instance; // Singleton instance reference
    public GameObject prefab; // Prefab of the enemy
    public int initialPoolSize; // Initial size of the pool

    private Queue<GameObject> pool = new Queue<GameObject>(); // Queue for inactive enemy objects
    private List<GameObject> activeObjects = new List<GameObject>(); // List for active enemy objects

    void Awake() {
        // Singleton pattern to ensure only one instance exists
        if (instance == null) {
            instance = this;
        }
        else {
            Destroy(gameObject); // Destroy duplicates of the singleton
        }
    }

    void Start() {
        // Initialize the object pool with the specified initial size
        for (int i = 0; i < initialPoolSize; i++) {
            GameObject obj = Instantiate(prefab, transform);
            obj.SetActive(false);
            pool.Enqueue(obj);
        }
    }

    /// <summary>
    /// Retrieves an enemy object from the pool.
    /// </summary>
    /// <returns>An inactive enemy object from the pool, or a newly instantiated one if the pool is empty.</returns>
    public GameObject GetObjectFromPool() {
        GameObject obj = null;

        // Check if there are any inactive objects in the pool
        if (pool.Count > 0) {
            obj = pool.Dequeue(); // Dequeue an inactive object
            obj.SetActive(true); // Activate the object
            activeObjects.Add(obj); // Add to the list of active objects
        }
        else {
            // If the pool is empty, instantiate a new object
            obj = Instantiate(prefab, transform);
            activeObjects.Add(obj); // Add to the list of active objects
        }

        return obj;
    }

    /// <summary>
    /// Returns an enemy object to the pool.
    /// </summary>
    /// <param name="obj">The enemy object to return to the pool.</param>
    public void ReturnObjectToPool(GameObject obj) {
        obj.SetActive(false); // Deactivate the object
        activeObjects.Remove(obj); // Remove from the list of active objects
        pool.Enqueue(obj); // Enqueue the object back into the pool
    }
}
