using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Represents an item in the inventory that can be dragged and dropped.
/// </summary>
[RequireComponent(typeof(Image))]
public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {
    public bool[,] GridRepresentation { get; private set; }
    public static event Action<Item> OnSetItem;
    public static event Action<Item> OnRemoveItem;


    private Item m_item;
    private Transform m_originalParent;
    private Image m_image;
    private Transform[] m_childrenTransforms;
    private List<InventorySlot> m_usedSlots = new List<InventorySlot>();
    private bool m_isBeingDragged = false;

    private void Start() {
        m_image = GetComponent<Image>();
        m_childrenTransforms = new Transform[transform.childCount];
        m_originalParent = transform.parent;
        int i = 0;
        foreach (Transform child in transform) {
            m_childrenTransforms[i] = child;
            child.GetComponent<Image>().color = m_item.getColor();
            i++;
        }
        create2DArray(m_childrenTransforms);
    }

    private void Update() {
        rotateItem();
    }

    /// <summary>
    /// Called when the item begins to be dragged.
    /// </summary>
    /// <param name="t_eventData">Pointer event data.</param>
    public void OnBeginDrag(PointerEventData t_eventData) {
        transform.SetParent(transform.root);
        transform.SetAsLastSibling(); // Move to the last position of children to be visible in UI
        m_image.raycastTarget = false; // To prevent interacting with this image while dragging
        setOccupyingSlots(false);
    }

    /// <summary>
    /// Called when the item is being dragged.
    /// </summary>
    /// <param name="t_eventData">Pointer event data.</param>
    public void OnDrag(PointerEventData t_eventData) {
        transform.position = Input.mousePosition;
        m_isBeingDragged = true;
    }

    /// <summary>
    /// Called when the item stops being dragged.
    /// </summary>
    /// <param name="t_eventData">Pointer event data.</param>
    public void OnEndDrag(PointerEventData t_eventData) {
        m_isBeingDragged = false;
        m_image.raycastTarget = true;
        if (transform.parent == transform.root) {
            transform.SetParent(m_originalParent);
            setOccupyingSlots(true);
        } // Reset position to last parent if current parent is root.
        transform.localPosition = Vector3.zero;
        if (m_originalParent.GetComponent<InventorySlot>()) {
            transform.SetParent(m_originalParent.parent);
        }
    }

    /// <summary>
    /// Sets the slots that the item occupies.
    /// </summary>
    /// <param name="t_occupiedSlots">List of slots that the item occupies.</param>
    public void setOccupiedSlots(List<InventorySlot> t_occupiedSlots) {
        m_usedSlots = t_occupiedSlots;
    }

    /// <summary>
    /// Sets the parent transform of the item.
    /// </summary>
    /// <param name="t_parentTransform">Parent transform to set.</param>
    public void setParent(Transform t_parentTransform) {
        m_originalParent = t_parentTransform;
    }

    public void setItem(Item t_item) {
        m_item = t_item;
    }

    /// <summary>
    /// Prints the grid representation of the item to the console.
    /// </summary>
    public void printGrid() {
        int rows = GridRepresentation.GetLength(0);
        int cols = GridRepresentation.GetLength(1);
        string arrayString = "";
        for (int i = 0; i < rows; i++) {
            for (int j = 0; j < cols; j++) {
                arrayString += $"{GridRepresentation[i, j]}";
            }
            arrayString += "\n";
        }
        Debug.Log(arrayString);
    }

    /// <summary>
    /// Sets the occupancy status of the slots the item occupies.
    /// </summary>
    /// <param name="t_status">True to set slots as occupied, false to set as unoccupied.</param>
    private void setOccupyingSlots(bool t_status) {
        foreach (InventorySlot slot in m_usedSlots) {
            slot.isOccupied = t_status;
        }
        if (m_usedSlots.Count == 0) {
            return;
        }
        if (t_status) {
            OnSetItem?.Invoke(m_item);
        }
        else {
            OnRemoveItem?.Invoke(m_item);
        }
    }

    /// <summary>
    /// Creates a 2D array representing the grid space occupied by the item.
    /// </summary>
    /// <param name="t_childrenTransforms">Array of child transforms of the item.</param>
    private void create2DArray(Transform[] t_childrenTransforms) {
        int gridSpace = (int)Mathf.Sqrt(transform.childCount);
        GridRepresentation = new bool[gridSpace, gridSpace];
        for (int i = 0; i < gridSpace; i++) {
            for (int j = 0; j < gridSpace; j++) {
                GridRepresentation[i, j] = t_childrenTransforms[i * gridSpace + j].gameObject.activeInHierarchy;
            }
        }
    }

    /// <summary>
    /// Handles the rotation of the item based on user input.
    /// </summary>
    private void rotateItem() {
        if (!m_isBeingDragged) {
            return;
        }
        if (Input.GetKeyUp(KeyCode.Q)) {
            rotateMatrixCounterClockwise();
        }
        if (Input.GetKeyUp(KeyCode.E)) {
            rotateMatrixClockwise();
        }
    }

    /// <summary>
    /// Rotates the grid representation of the item counter-clockwise.
    /// </summary>
    private void rotateMatrixCounterClockwise() {
        int rows = GridRepresentation.GetLength(0);
        int cols = GridRepresentation.GetLength(1);
        bool[,] rotatedMatrix = new bool[rows, cols];
        for (int i = 0; i < rows; i++) {
            for (int j = 0; j < cols; j++) {
                rotatedMatrix[i, j] = GridRepresentation[j, rows - 1 - i];
            }
        }
        GridRepresentation = rotatedMatrix;
        updateVisuals();
    }

    /// <summary>
    /// Rotates the grid representation of the item clockwise.
    /// </summary>
    private void rotateMatrixClockwise() {
        int rows = GridRepresentation.GetLength(0);
        int cols = GridRepresentation.GetLength(1);
        bool[,] rotatedMatrix = new bool[rows, cols];
        for (int i = 0; i < rows; i++) {
            for (int j = 0; j < cols; j++) {
                rotatedMatrix[i, j] = GridRepresentation[cols - 1 - j, i];
            }
        }
        GridRepresentation = rotatedMatrix;
        updateVisuals();
    }


    /// <summary>
    /// Updates the visual representation of the item based on the grid representation.
    /// </summary>
    private void updateVisuals() {
        // Might not be necessary.
        int rows = GridRepresentation.GetLength(0);
        int cols = GridRepresentation.GetLength(1);
        int gridSpace = (int)Mathf.Sqrt(transform.childCount);
        for (int i = 0; i < rows; i++) {
            for (int j = 0; j < cols; j++) {
                m_childrenTransforms[i * gridSpace + j].gameObject.SetActive(GridRepresentation[i, j]);
            }
        }
    }
}