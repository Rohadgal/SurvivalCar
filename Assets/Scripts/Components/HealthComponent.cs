using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

/// <summary>
/// Manages the health of a game object, including taking damage, healing, and triggering events on death.
/// </summary>
public class HealthComponent : MonoBehaviour {
    public float CurrentHealth { get; private set; }
    
    [SerializeField] private float maxHealth = 100;
    [FormerlySerializedAs("OnDeadEvent")] [SerializeField] private UnityEvent onDeadEvent;

    private void Start() {
        initialize();
    }

    public void initialize() {
        CurrentHealth = maxHealth;
    }

    /// <summary>
    /// Returns the current max health of this unit.
    /// </summary>
    /// <returns>Returns the current max health of this unit.</returns>
    public float getMaxHealth() {
        return maxHealth;
    }

    /// <summary>
    /// Reduces the current health by a specified amount.
    /// If health falls below or equal to zero, triggers the die method.
    /// </summary>
    /// <param name="t_amount">The amount of damage to take.</param>
    public void takeDamage(float t_amount) {
        CurrentHealth -= t_amount;
        if (CurrentHealth <= 0) {
            die();
        }
    }

    /// <summary>
    /// Increases the current health by a specified amount.
    /// Ensures that current health does not exceed maximum health.
    /// </summary>
    /// <param name="t_amount">The amount of health to restore.</param>
    public void heal(float t_amount) {
        CurrentHealth += t_amount;
        if (CurrentHealth > maxHealth) {
            CurrentHealth = maxHealth;
        }
    }

    /// <summary>
    /// Increases the maximum health by a specified amount.
    /// </summary>
    /// <param name="t_amountToIncrease">The amount to increase the maximum health by.</param>
    public void increaseMaxHealth(float t_amountToIncrease) {
        maxHealth += t_amountToIncrease;
    }

    /// <summary>
    /// Decreases the maximum health by a specified amount.
    /// </summary>
    /// <param name="t_amountToDecrease">The amount to decrease the maximum health by.</param>
    public void decreaseMaxHealth(float t_amountToDecrease) {
        maxHealth -= t_amountToDecrease;
    }

    /// <summary>
    /// Handles the game object's death.
    /// Invokes the OnDeadEvent and deactivates the game object.
    /// </summary>
    private void die() {
        onDeadEvent?.Invoke();
    }
}
