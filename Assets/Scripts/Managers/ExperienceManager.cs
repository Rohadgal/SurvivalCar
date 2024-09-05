using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// Manages the experience points of the player.
/// </summary>
[RequireComponent(typeof(ExperienceFactory))]
public class ExperienceManager : MonoBehaviour {
    public static ExperienceManager s_instance;
    public static event Action OnLevelUp;
    public int ExperienceNeededToLevelUp { get; private set; }

    [SerializeField] private int initialExperienceToLevelUp = 50;

    [SerializeField, Tooltip("The factor by which the experience needed to level up scales.")]
    private float experienceScale = 1.5f;

    [SerializeField] private GameObject experiencePrefab;

    private int m_currentExperience = 0;
    private readonly List<GameObject> m_pool = new List<GameObject>();

    private void Awake() {
        if (FindObjectOfType<ExperienceManager>() != null &&
            FindObjectOfType<ExperienceManager>().gameObject != gameObject) {
            Destroy(gameObject);
            return;
        }
        s_instance = this;
        resetExperience();
        ExperienceNeededToLevelUp = initialExperienceToLevelUp;
    }

    private void OnEnable() {
        ExperienceCollectible.OnAnyExperienceCollected += addExperience;
        EnemyController.OnDead += spawnExperience;
    }

    private void OnDisable() {
        ExperienceCollectible.OnAnyExperienceCollected -= addExperience;
        EnemyController.OnDead -= spawnExperience;
    }

    private void Start() {
        resetExperience();
    }

    /// <summary>
    /// Spawns an experience collectible at the specified position.
    /// </summary>
    /// <param name="t_spawnPosition">The position to spawn the experience collectible.</param>
    public void spawnExperience(Vector3 t_spawnPosition) {
#if !UNITY_EDITOR
        if (Random.value >= 0.5f) {
            return;
        }
#endif
        
        foreach (GameObject experience in m_pool.Where(t_experience => !t_experience.activeInHierarchy)) {
            experience.transform.position = t_spawnPosition;
            experience.SetActive(true);
            return;
        }
        GameObject newExperience = ExperienceFactory.s_instance.spawnExperience(experiencePrefab);
        newExperience.transform.position = t_spawnPosition;
        newExperience.transform.SetParent(transform);
        m_pool.Add(newExperience);
    }

    /// <summary>
    /// Resets the current experience points to zero.
    /// </summary>
    private void resetExperience() {
        m_currentExperience = 0;
    }

    /// <summary>
    /// Adds experience points and handles leveling up.
    /// </summary>
    /// <param name="t_experienceToAdd">The amount of experience to add.</param>
    private void addExperience(int t_experienceToAdd) {
        while (true) {
            m_currentExperience += t_experienceToAdd;
            if (m_currentExperience >= ExperienceNeededToLevelUp) {
                int experienceSurplus = ExperienceNeededToLevelUp - m_currentExperience;
                resetExperience();
                ExperienceNeededToLevelUp = Mathf.RoundToInt(ExperienceNeededToLevelUp * experienceScale);
                t_experienceToAdd = experienceSurplus;
                OnLevelUp?.Invoke();
                continue;
            }
            break;
        }
    }
}