using UnityEngine;

public class TireMovement : MonoBehaviour {
    private float m_damage;
    private float m_countdown;

    private void OnEnable() {
        m_countdown = 20;
    }

    public void setDamage(float t_newDamage) {
        m_damage = t_newDamage;
    }
    
    private void Update() {
        m_countdown -= Time.deltaTime;
        if (m_countdown <= 0) {
            gameObject.SetActive(false);
        }
    }

    private void OnCollisionEnter(Collision t_other) {
        if (t_other.gameObject.CompareTag("Zombie")) {
            t_other.gameObject.GetComponent<EnemyController>().takeDamage(m_damage);
            gameObject.SetActive(false);
        }
    }
}