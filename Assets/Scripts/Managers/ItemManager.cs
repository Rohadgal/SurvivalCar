using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// Manages a collection of items that implement the <see cref="IPowerUp"/> interface.
/// </summary>
public class ItemManager : MonoBehaviour {
    [FormerlySerializedAs("testItem")] [SerializeField] private MachineGun startingItem;

    private readonly List<IPowerUp> m_items = new List<IPowerUp>();

    private void OnEnable() {
        InventoryItem.OnSetItem += addItem;
        InventoryItem.OnRemoveItem += removeItem;
    }
    
    private void OnDisable() {
        InventoryItem.OnSetItem -= addItem;
        InventoryItem.OnRemoveItem -= removeItem;
    }

    private void Start() {
        if (startingItem == null) {
            return;
        }
        addItem((IPowerUp)startingItem);
    }

    /// <summary>
    /// Unity's Update method, called once per frame.
    /// </summary>
    private void Update() {
        foreach (IPowerUp t in m_items) {
            t.update();
        }
    }

    private void addItem(Item t_item) {
        addItem((IPowerUp)t_item);
    }

    /// <summary>
    /// Adds an item to the manager and initializes it.
    /// </summary>
    /// <param name="t_item">The item to add.</param>
    private void addItem(IPowerUp t_item) {
        t_item.initialize(gameObject);
        m_items.Add(t_item);
    }

    private void removeItem(Item t_item) {
        removeItem((IPowerUp)t_item);
    }
    
    /// <summary>
    /// Removes an item from the manager and exits it.
    /// </summary>
    /// <param name="t_item">The item to remove.</param>
    private void removeItem(IPowerUp t_item) {
        IPowerUp tempItem = m_items.Find(item => item == t_item);
        if (tempItem == null) {
            return;
        }
        tempItem.exit();
        m_items.Remove(tempItem);
    }
}