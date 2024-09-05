using UnityEngine;

/// <summary>
/// Controls the movement of a seeking missile.
/// </summary>
[RequireComponent(typeof(Rigidbody), typeof(BoxCollider))]
public class SeekingMissileMovement : MonoBehaviour {
    [SerializeField] private float movementSpeed = 15f;
    
    private Rigidbody m_rb;
    private Transform m_targetTransform;
    
    private void Start() {
        m_rb = GetComponent<Rigidbody>();
        m_rb.useGravity = false;
    }
    
    private void FixedUpdate() {
        if (!m_targetTransform) {
            return;
        }
        Vector3 positionChange = m_targetTransform.position - transform.position;
        float time = positionChange.magnitude / movementSpeed;
        m_rb.velocity = positionChange / Mathf.Max(time, Time.fixedDeltaTime);
    }
}