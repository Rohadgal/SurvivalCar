using AudioSystem;
using System;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;
using UnityEngine.VFX;

/// <summary>
/// Controls the behavior of an enemy character, including animations, health, and interactions with other game objects.
/// </summary>
/// <remarks>
/// This class manages the state, animations, and interactions of an enemy character in the game.
/// It handles the enemy's skin selection, animation states, health management, and interaction with the player,
/// such as dealing with damage and reacting to being hit or killed.
/// The class also triggers audio feedback for certain events like taking damage or dying.
/// </remarks>
[RequireComponent(typeof(HealthComponent))]
public class EnemyController : MonoBehaviour {
    /// <summary>
    /// Event triggered when the enemy dies.
    /// </summary>
    public static event Action<Vector3> OnDead;
    
    [FormerlySerializedAs("clips")] [SerializeField]
    private AudioClip[] deadAudioClips;
    [SerializeField] private SoundData deadSoundData;
    [SerializeField] private AudioClip[] hitAudioClips;
    [FormerlySerializedAs("soundData")] [SerializeField]
    private SoundData hitSoundData;
    [SerializeField] private GameObject[] selectableSkins;
    [SerializeField] private EnemyState currentState;
  //  [SerializeField] private GameObject bloodGO;

    /*[SerializeField]*/ VisualEffect bloodVFX;
    private LevelManager levelManager;
    private HealthComponent m_healthComponent;
    private Animator m_animator;
    private bool isShot = false;
    
    /// <summary>
    /// Defines the possible animation states for the enemy.
    /// </summary>
    private enum EnemyState {
        Run1,
        Run2,
        Run3
    }
    
    private void Start() {
        m_healthComponent = GetComponent<HealthComponent>();
        m_animator = GetComponent<Animator>();
        activateRandomSkin();
        chooseRandomState();
        changeEnemyAnimation();
        //bloodGO.SetActive(true);
        bloodVFX = GetComponent<VisualEffect>();
        
    }
    
    private void OnEnable() {
        if (m_healthComponent != null) {
            m_healthComponent.initialize();
        }
    }

    /// <summary>
    /// Ensures the correct animation is playing based on the current state.
    /// </summary>
    private void rectifyAnimation() {
        switch (currentState) {
            case EnemyState.Run1:
                if (!m_animator.GetBool("Run1")) {
                    changeEnemyAnimation();
                }
                break;
            case EnemyState.Run2:
                if (!m_animator.GetBool("Run2")) {
                    changeEnemyAnimation();
                }
                break;
            case EnemyState.Run3:
                if (!m_animator.GetBool("Run3")) {
                    changeEnemyAnimation();
                }
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Selects a random animation state for the enemy.
    /// </summary>
    private void chooseRandomState() {
        int numberOfStates = Enum.GetValues(typeof(EnemyState)).Length;
        int randomStateIndex = UnityEngine.Random.Range(0, numberOfStates);
        currentState = (EnemyState)randomStateIndex;
    }

    /// <summary>
    /// Updates the animator to reflect the current animation state.
    /// </summary>
    private void changeEnemyAnimation() {
        m_animator.SetBool("Run1", false);
        m_animator.SetBool("Run2", false);
        m_animator.SetBool("Run3", false);
        switch (currentState) {
            case EnemyState.Run1:
                m_animator.SetBool("Run1", true);
                break;
            case EnemyState.Run2:
                m_animator.SetBool("Run2", true);
                break;
            case EnemyState.Run3:
                m_animator.SetBool("Run3", true);
                break;
            default:
                rectifyAnimation();
                break;
        }
    }

    /// <summary>
    /// Activates a random skin from the selectable skins.
    /// </summary>
    private void activateRandomSkin() {
        foreach (Transform child in transform) {
            child.gameObject.SetActive(false);
        }
        GameObject randomSkin = getRandomSkin();
        if (randomSkin != null) {
            randomSkin.SetActive(true);
        }
    }

    /// <summary>
    /// Retrieves a random skin from the selectable skins array.
    /// </summary>
    /// <returns>The randomly selected skin, or null if no skins are available.</returns>
    private GameObject getRandomSkin() {
        if (selectableSkins == null || selectableSkins.Length == 0) {
            Debug.LogWarning("No skins selected for enemy.");
            return null;
        }
        int randomIndex = Random.Range(0, selectableSkins.Length);
        return selectableSkins[randomIndex];
    }

    /// <summary>
    /// Applies damage to the enemy and plays the hit sound.
    /// </summary>
    /// <param name="t_damageReceived">The amount of damage to apply.</param>
    public void takeDamage(float t_damageReceived) {
        if (!isShot) {
            isShot = true;
            bloodVFX.Play();
        }
        m_healthComponent.takeDamage(t_damageReceived);
        playHitSound(true);
    }

    /// <summary>
    /// Handles the enemy's death, triggering the OnDead event and playing the death sound.
    /// </summary>
    public void invokeZombieDeath() {
        OnDead?.Invoke(transform.position);
        deadSoundData.clip = deadAudioClips[Random.Range(0, deadAudioClips.Length)];
        SoundManager.Instance.createSoundBuilder().withRandomPitch().play(deadSoundData);
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Handles collision with other objects, specifically dealing damage to the player on contact.
    /// </summary>
    /// <param name="t_collision">The collision data.</param>
    private void OnCollisionEnter(Collision t_collision) {
        if (t_collision.gameObject.CompareTag("Player")) {
            t_collision.gameObject.GetComponent<PlayerManager>().takeDamage(5f);
            ObjectEnemyPool.instance.ReturnObjectToPool(gameObject);
            playHitSound();
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Plays the hit sound, optionally with a random chance of not playing.
    /// </summary>
    /// <param name="t_random">If true, the sound will only play based on a 50/50 random chance.</param>
    private void playHitSound(bool t_random = false) {
        if (t_random) {
            if (Random.value > 0.5f) {
                return;
            }
        }
        hitSoundData.clip = hitAudioClips[Random.Range(0, hitAudioClips.Length)];
        SoundManager.Instance.createSoundBuilder().withRandomPitch().play(hitSoundData);
    }
}