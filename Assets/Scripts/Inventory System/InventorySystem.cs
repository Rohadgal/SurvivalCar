using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the inventory system, including slot initialization.
/// </summary>
public class InventorySystem : MonoBehaviour {
    public static InventorySystem s_instance;
    
    private InventorySlot[,] m_slots;
    
    private void Start() {
        InventorySlot[] childSlots = new InventorySlot[transform.childCount];
        int i = 0;
        foreach (Transform child in transform) {
            childSlots[i] = child.GetComponent<InventorySlot>();
            i++;
        }
        create2DArray(childSlots);
    }

    /// <summary>
    /// Checks if an item can be placed at the specified grid position.
    /// </summary>
    /// <param name="t_gridRepresentation">2D array representing the item grid.</param>
    /// <param name="t_xPos">X position in the inventory grid.</param>
    /// <param name="t_yPos">Y position in the inventory grid.</param>
    /// <param name="t_usedSlots">Adjacent slots used by the item</param>
    /// <returns>True if the item can be placed, false otherwise.</returns>
    public bool isItemPlaceable(bool[,] t_gridRepresentation, int t_xPos, int t_yPos,
        out List<InventorySlot> t_usedSlots) {
        t_usedSlots = new List<InventorySlot>();
        int itemGridSize = t_gridRepresentation.GetLength(0); // Size of the item grid.
        int center = itemGridSize / 2; // Center of the item grid.
        // int xItemStartPos = center - 1; // Start x position of the item grid (should be 0).
        // int yItemStartPos = center - 1; // Start y position of the item grid (should be 0).
        // Loop the inventory grid and also the item grid.
        // If a slot in the inventory is occupied and the item grid wants to occupy
        // that cell returns false, else returns true.
        for (int x = t_xPos - 1, xItemStartPos = center - 1; x <= t_xPos + 1; x++, xItemStartPos++) {
            for (int y = t_yPos - 1, yItemStartPos = center - 1; y <= t_yPos + 1; y++, yItemStartPos++) {
                if (xItemStartPos >= itemGridSize || yItemStartPos >= itemGridSize) {
                    continue;
                }
                if (isOccupied(x, y) && t_gridRepresentation[xItemStartPos, yItemStartPos]) {
                    return false; // Returns false because item can't be placed here.
                }
                else if (t_gridRepresentation[xItemStartPos, yItemStartPos]) {
                    t_usedSlots.Add(m_slots[x, y]);
                }
            }
        }
        return true; // Return true because item can be placed here.
    }

    /// <summary>
    /// Creates a 2D array of inventory slots from the given array.
    /// </summary>
    /// <param name="t_inventorySlots">Array of inventory slots.</param>
    private void create2DArray(InventorySlot[] t_inventorySlots) {
        int gridSpace = (int)Mathf.Sqrt(transform.childCount);
        m_slots = new InventorySlot[gridSpace, gridSpace];
        for (int x = 0; x < gridSpace; x++) {
            for (int y = 0; y < gridSpace; y++) {
                m_slots[x, y] = t_inventorySlots[x * gridSpace + y];
                m_slots[x, y].setGridPosition(x, y);
            }
        }
    }

    /// <summary>
    /// Checks if a slot at the specified position is occupied.
    /// </summary>
    /// <param name="t_xPos">X position in the inventory grid.</param>
    /// <param name="t_yPos">Y position in the inventory grid.</param>
    /// <returns>True if the slot is occupied, false otherwise.</returns>
    private bool isOccupied(int t_xPos, int t_yPos) {
        if (t_xPos < 0 || t_xPos >= m_slots.GetLength(0) ||
            t_yPos < 0 || t_yPos >= m_slots.GetLength(0)) {
            return true;
        }
        return m_slots[t_xPos, t_yPos].isOccupied;
    }
}