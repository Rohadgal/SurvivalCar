using UnityEngine;

[RequireComponent(typeof(AudioSource), typeof(Rigidbody))]
public class PlayerSoundController : MonoBehaviour {
    [SerializeField] private float minSpeed;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float minPitch;
    [SerializeField] private float maxPitch;

    private Rigidbody m_rb;
    private AudioSource m_audioSource;
    private float m_pitchFromCar;
    private float m_currentSpeed;

    private void Start() {
        m_audioSource = GetComponent<AudioSource>();
        m_rb = GetComponent<Rigidbody>();
    }

    private void Update() {
        engineSound();
    }

    private void engineSound() {
        m_currentSpeed = m_rb.velocity.magnitude;
        m_pitchFromCar = m_rb.velocity.magnitude / 50f;
        if (m_currentSpeed < minSpeed) {
            m_audioSource.pitch = minPitch;
        }
        else if (m_currentSpeed > minSpeed && m_currentSpeed < maxSpeed) {
            m_audioSource.pitch = minPitch + m_pitchFromCar;
        }
        else if (m_currentSpeed > maxSpeed) {
            m_audioSource.pitch = maxPitch;
        }
    }
}