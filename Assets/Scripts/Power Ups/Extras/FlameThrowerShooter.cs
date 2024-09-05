using UnityEngine;
using UnityEngine.VFX;

public class FlameThrowerShooter : MonoBehaviour
{
    [SerializeField] private VisualEffect flameThrowerEffect;
    [SerializeField] private float m_damage;

    private void OnTriggerStay(Collider t_other)
    {
        if (t_other.CompareTag("Zombie"))
        {
            t_other.GetComponent<EnemyController>().takeDamage(m_damage);
            Debug.Log("Damage");
        }
    }
}