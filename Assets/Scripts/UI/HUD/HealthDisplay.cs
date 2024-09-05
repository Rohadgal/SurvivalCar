using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Displays the player's health using a UI slider. Updates the slider based on health changes.
/// </summary>
public class HealthDisplay : MonoBehaviour {
    private Slider m_slider;
    
    private void OnEnable() {
        m_slider = GetComponent<Slider>();
        PlayerManager.OnPlayerHealthChanged += setHealth;
        PlayerManager.OnPlayerMaxHealthChanged += setMaxHealth;
    }
    
    private void OnDisable() {
        PlayerManager.OnPlayerHealthChanged -= setHealth;
        PlayerManager.OnPlayerMaxHealthChanged -= setMaxHealth;
    }
    
    private void Start() {
        resetHealth();
    }

    /// <summary>
    /// Updates the slider's value to reflect the player's current health.
    /// </summary>
    /// <param name="t_health">The current health value of the player.</param>
    private void setHealth(float t_health) {
        m_slider.value = t_health;
    }
    
    /// <summary>
    /// Updates the slider's maximum value to reflect the player's maximum health.
    /// </summary>
    /// <param name="t_maxHealth">The maximum health value of the player.</param>
    private void setMaxHealth(float t_maxHealth) {
        m_slider.maxValue = t_maxHealth;
    }

    /// <summary>
    /// Resets the slider's value and maximum value to the player's current and maximum health at the start.
    /// </summary>
    private void resetHealth() {
        m_slider.value = PlayerManager.s_instance.GetComponent<HealthComponent>().CurrentHealth;
        m_slider.maxValue = PlayerManager.s_instance.GetComponent<HealthComponent>().getMaxHealth();
    }
}
