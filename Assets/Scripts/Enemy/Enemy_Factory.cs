using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Factory : MonoBehaviour {
    public static Enemy_Factory Instance; // Singleton instance of the factory

    private void Awake() {
        Instance = this; // Assign the singleton instance
    }

    // Method to create an enemy object based on a prefab
    public GameObject CreateEnemy(GameObject prefab) {
        GameObject obj = Instantiate(prefab); // Instantiate the enemy prefab
        return obj; // Return the instantiated enemy object
    }
}
