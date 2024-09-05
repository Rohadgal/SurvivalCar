using System;
using AudioSystem;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// Represents a collectible item that grants experience when collected.
/// </summary>
[RequireComponent(typeof(BoxCollider))]
public class ExperienceCollectible : MonoBehaviour {
    public static event Action<int> OnAnyExperienceCollected;
    
    [SerializeField] private int experience;
    [FormerlySerializedAs("soundData")] [SerializeField] private SoundData pickUpSoundData;
    [SerializeField] private SoundData instantiateSoundData;
    
    private void Start() {
        GetComponent<BoxCollider>().isTrigger = true;
    }

    private void OnEnable() {
        SoundManager.Instance.createSoundBuilder().withRandomPitch().play(instantiateSoundData);
    }

    private void OnTriggerEnter(Collider t_other) {
        if (!t_other.gameObject.CompareTag("Player")) {
            return;
        }
        OnAnyExperienceCollected?.Invoke(experience);
        SoundManager.Instance.createSoundBuilder().withRandomPitch().play(pickUpSoundData);
        gameObject.SetActive(false);
    }
}