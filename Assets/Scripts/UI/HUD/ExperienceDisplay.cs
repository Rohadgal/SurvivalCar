using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Displays the player's experience using a UI slider.
/// </summary>
[RequireComponent(typeof(Slider))]
public class ExperienceDisplay : MonoBehaviour {
    private Slider m_slider;
    private ExperienceManager m_experienceManager;
    
    private void OnEnable() {
        ExperienceCollectible.OnAnyExperienceCollected += addExperience;
        ExperienceManager.OnLevelUp += resetExperienceFill;
    }
    
    private void OnDisable() {
        ExperienceCollectible.OnAnyExperienceCollected -= addExperience;
        ExperienceManager.OnLevelUp -= resetExperienceFill;
    }
    
    private void Start() {
        m_slider = GetComponent<Slider>();
        m_slider.wholeNumbers = true;
        m_experienceManager = ExperienceManager.s_instance;
        resetExperienceFill();
    }

    /// <summary>
    /// Adds experience to the slider value.
    /// </summary>
    /// <param name="t_experienceToAdd">The amount of experience to add.</param>
    private void addExperience(int t_experienceToAdd) {
        m_slider.value += t_experienceToAdd;
    }

    /// <summary>
    /// Resets the experience fill on the slider.
    /// </summary>
    private void resetExperienceFill() {
        m_slider.maxValue = m_experienceManager.ExperienceNeededToLevelUp;
        m_slider.value = 0;
    }
}