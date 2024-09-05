using UnityEngine;

/// <summary>
/// Represents a generic item in the game.
/// </summary>
public class Item : ScriptableObject {
    public string itemName = "Item name";
    public string itemDescription = "Item description";
    public GameObject UIRepresentation;

    public virtual Color getColor() {
        throw new System.NotImplementedException();
    }
}

/// <summary>
/// Interface for implementing power-ups in the game.
/// </summary>
public interface IPowerUp {
    /// <summary>
    /// Initializes the power-up.
    /// </summary>
    void initialize(GameObject t_parent);

    /// <summary>
    /// Updates the power-up state.
    /// </summary>
    void update();

    /// <summary>
    /// Cleans up and exits the power-up.
    /// </summary>
    void exit();
}