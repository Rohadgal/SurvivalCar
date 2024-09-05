using UnityEngine;

/// <summary>
/// Represents a speed boost power-up in the game, which temporarily increases the player's speed.
/// </summary>
[CreateAssetMenu(fileName = "Speed Boost", menuName = "CarsSurvivor/Speed Boost")]
public class SpeedBoost : Item, IPowerUp {
    /// <summary>
    /// The amount by which the player's speed will be increased.
    /// </summary>
    [SerializeField] private float speedIncrease = 500f;
    
    /// <summary>
    /// The color representing the speed boost item.
    /// </summary>
    private readonly Color m_tileColor = new Color(0.031f, 0.403f, 0.533f);

    /// <summary>
    /// Initializes the speed boost, increasing the player's speed.
    /// </summary>
    /// <param name="t_parent">The parent GameObject, typically the player.</param>
    public void initialize(GameObject t_parent) {
        PlayerManager.s_instance.increaseSpeed(speedIncrease);
    }

    /// <summary>
    /// Updates the speed boost. This method is empty as the speed boost does not require updates.
    /// </summary>
    public void update() {
        // Empty because doesn't do anything.
    }

    /// <summary>
    /// Exits the speed boost, returning the player's speed to normal.
    /// </summary>
    public void exit() {
        PlayerManager.s_instance.decreaseSpeed(speedIncrease);
    }
    
    /// <summary>
    /// Gets the color associated with the speed boost item.
    /// </summary>
    /// <returns>The color representing the speed boost.</returns>
    public override Color getColor() {
        return m_tileColor;
    }
}
