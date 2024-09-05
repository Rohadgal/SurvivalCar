using UnityEngine;

/// <summary>
/// Represents a shield power-up in the game, which provides temporary protection to the player.
/// </summary>
[CreateAssetMenu(fileName = "Shield", menuName = "CarsSurvivor/Shield")]
public class Shield : Item, IPowerUp {
    /// <summary>
    /// The color representing the shield item.
    /// </summary>
    private readonly Color m_tileColor = new Color(0.486f, 0.596f, 0.282f);

    /// <summary>
    /// Initializes the shield power-up. Typically involves activating the shield on the player.
    /// </summary>
    /// <param name="t_parent">The parent GameObject, usually the player.</param>
    public void initialize(GameObject t_parent) {
        Debug.Log("Spawn shield");
    }

    /// <summary>
    /// Updates the shield power-up. This method is currently empty and doesn't perform any actions.
    /// </summary>
    public void update() {
    }

    /// <summary>
    /// Exits the shield power-up, typically deactivating the shield on the player.
    /// This method is currently empty and doesn't perform any actions.
    /// </summary>
    public void exit() {
    }
    
    /// <summary>
    /// Gets the color associated with the shield item.
    /// </summary>
    /// <returns>The color representing the shield.</returns>
    public override Color getColor() {
        return m_tileColor;
    }
}