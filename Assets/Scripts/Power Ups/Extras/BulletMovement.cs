using Unity.Mathematics;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BulletMovement : MonoBehaviour {
    [SerializeField] private float travelSpeed = 15f;
    [SerializeField] private float timeAliveInSeconds = 20f;
    
    private Vector3 m_direction;
    public GameObject particleSystem;
    private Quaternion _rotation;
    private Rigidbody m_rb;
    private float m_countdown;
    private float m_damage = 50;

    private void Start() {
        m_rb = GetComponent<Rigidbody>();
        m_rb.useGravity = false;
        
        m_countdown = timeAliveInSeconds;
    }

    private void OnEnable() {
        m_direction = PlayerManager.s_instance.transform.forward;
        _rotation = Quaternion.LookRotation(PlayerManager.s_instance.transform.right);
        
        m_countdown = timeAliveInSeconds;
    }


    private void Update() {
        m_countdown -= Time.deltaTime;
        if (m_countdown <= 0) {
            gameObject.SetActive(false);
        }
    }

    private void FixedUpdate() {
        m_rb.velocity = m_direction * travelSpeed;
        particleSystem.transform.rotation = _rotation;
    }

    private void OnTriggerEnter(Collider t_other) {
        if (t_other.CompareTag("Zombie")) {
            t_other.GetComponent<EnemyController>().takeDamage(m_damage);
            gameObject.SetActive(false);
        }
    }

    public void updateDamage(float t_newDamage) {
        m_damage = t_newDamage;
    }
}