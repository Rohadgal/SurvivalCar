using System;
using UnityEngine;

/// <summary>
/// Manages the player's health, speed, and overall state within the game.
/// Handles player-related events and interactions with other components.
/// </summary>
[RequireComponent(typeof(HealthComponent), typeof(Player_Controller))]
public class PlayerManager : MonoBehaviour {
    /// <summary>
    /// Singleton instance of the PlayerManager.
    /// </summary>
    public static PlayerManager s_instance;

    /// <summary>
    /// Event triggered when the player's health changes.
    /// </summary>
    public static event Action<float> OnPlayerHealthChanged;

    /// <summary>
    /// Event triggered when the player's maximum health changes.
    /// </summary>
    public static event Action<float> OnPlayerMaxHealthChanged;

    private HealthComponent m_healthComponent;
    private Player_Controller m_playerController;
    
    private void Awake() {
        if (FindObjectOfType<PlayerManager>() != null &&
            FindObjectOfType<PlayerManager>().gameObject != gameObject) {
            Destroy(gameObject);
            return;
        }
        s_instance = this;
    }
    
    private void Start() {
        m_healthComponent = GetComponent<HealthComponent>();
        m_playerController = GetComponent<Player_Controller>();
        invokeOnPlayerHealthChanged();
        invokeOnPlayerMaxHealthChanged();
    }

    /// <summary>
    /// Reduces the player's health by a specified amount and triggers the health change event.
    /// </summary>
    /// <param name="t_damageToTake">Amount of damage to apply to the player.</param>
    public void takeDamage(float t_damageToTake) {
        m_healthComponent.takeDamage(t_damageToTake);
        invokeOnPlayerHealthChanged();
    }

    /// <summary>
    /// Heals the player by a specified amount and triggers the health change event.
    /// </summary>
    /// <param name="t_amountToHeal">Amount of health to restore to the player.</param>
    public void heal(float t_amountToHeal) {
        m_healthComponent.heal(t_amountToHeal);
        invokeOnPlayerHealthChanged();
    }

    /// <summary>
    /// Increases the player's maximum health by a specified amount and triggers the max health change event.
    /// </summary>
    /// <param name="t_amountToIncrease">Amount to increase the player's maximum health by.</param>
    public void increaseMaxHealth(float t_amountToIncrease) {
        m_healthComponent.increaseMaxHealth(t_amountToIncrease);
        invokeOnPlayerMaxHealthChanged();
    }

    /// <summary>
    /// Decreases the player's maximum health by a specified amount and triggers the max health change event.
    /// </summary>
    /// <param name="t_amountToDecrease">Amount to decrease the player's maximum health by.</param>
    public void decreaseMaxHealth(float t_amountToDecrease) {
        m_healthComponent.decreaseMaxHealth(t_amountToDecrease);
        invokeOnPlayerMaxHealthChanged();
    }

    /// <summary>
    /// Increases the player's speed by a specified amount.
    /// </summary>
    /// <param name="t_amountToIncrease">Amount to increase the player's speed by.</param>
    public void increaseSpeed(float t_amountToIncrease) {
        m_playerController.increaseMotorForce(t_amountToIncrease);
    }

    /// <summary>
    /// Decreases the player's speed by a specified amount.
    /// </summary>
    /// <param name="t_amountToDecrease">Amount to decrease the player's speed by.</param>
    public void decreaseSpeed(float t_amountToDecrease) {
        m_playerController.decreaseMotorForce(t_amountToDecrease);
    }

    /// <summary>
    /// Handles the player's death by transitioning to the credits level state.
    /// </summary>
    public void deadPlayer() {
        LevelManager.s_instance.setLevelState(LevelState.Credits);
    }

    /// <summary>
    /// Invokes the OnPlayerHealthChanged event with the current health value.
    /// </summary>
    private void invokeOnPlayerHealthChanged() {
        OnPlayerHealthChanged?.Invoke(m_healthComponent.CurrentHealth);
    }

    /// <summary>
    /// Invokes the OnPlayerMaxHealthChanged event with the current maximum health value.
    /// </summary>
    private void invokeOnPlayerMaxHealthChanged() {
        OnPlayerMaxHealthChanged?.Invoke(m_healthComponent.getMaxHealth());
    }
}
