using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Represents a slot in the inventory system where items can be placed.
/// </summary>
[RequireComponent(typeof(Image))]
public class InventorySlot : MonoBehaviour, IDropHandler {
    public bool isOccupied;
    public int GridX { get; private set; }
    public int GridY { get; private set; }
    
    private InventorySystem m_inventorySystem;
    
    private void Start() {
        m_inventorySystem = gameObject.GetComponentInParent<InventorySystem>();
    }

    /// <summary>
    /// Sets the grid position of the slot.
    /// </summary>
    /// <param name="t_xPosition">X position in the inventory grid.</param>
    /// <param name="t_yPosition">Y position in the inventory grid.</param>
    public void setGridPosition(int t_xPosition, int t_yPosition) {
        GridX = t_xPosition;
        GridY = t_yPosition;
    }

    /// <summary>
    /// Handles the drop event when an item is dropped onto this slot.
    /// </summary>
    /// <param name="t_eventData">Pointer event data.</param>
    public void OnDrop(PointerEventData t_eventData) {
        InventoryItem droppedObject = t_eventData.pointerDrag.GetComponent<InventoryItem>();
        if (!m_inventorySystem.isItemPlaceable(droppedObject.GridRepresentation, GridX, GridY,
                out List<InventorySlot> usedSlots)) {
            return;
        }
        droppedObject.setParent(transform);
        usedSlots.Add(this);
        foreach (InventorySlot slot in usedSlots) {
            slot.isOccupied = true;
        }
        droppedObject.setOccupiedSlots(usedSlots);
    }
}