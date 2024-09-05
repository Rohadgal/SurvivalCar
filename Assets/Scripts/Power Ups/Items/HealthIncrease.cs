using UnityEngine;

[CreateAssetMenu(fileName = "Health Increase", menuName = "CarsSurvivor/Health Increase")]
public class HealthIncrease : Item, IPowerUp {
    [SerializeField] private float healthIncrease = 50f;
    private readonly Color m_tileColor = new Color(1f, 0.945f,0.815f );

    public void initialize(GameObject t_parent) {
        PlayerManager.s_instance.increaseMaxHealth(healthIncrease);
    }

    public void update() {
        // Empty because doesn't do anything.
    }

    public void exit() {
        PlayerManager.s_instance.decreaseMaxHealth(healthIncrease);
    }
    
    public override Color getColor() {
        return m_tileColor;
    }
}