using UnityEngine;

[CreateAssetMenu(fileName = "Seeking Missile", menuName = "CarsSurvivor/Seeking Missile")]
public class SeekingMissile : Item, IPowerUp {
    private readonly Color m_tileColor = new Color(0.941f, 0.784f, 0.031f);

    public void initialize(GameObject t_parent) {
        throw new System.NotImplementedException();
    }

    public void update() {
        throw new System.NotImplementedException();
    }

    public void exit() {
        throw new System.NotImplementedException();
    }
    
    public override Color getColor() {
        return m_tileColor;
    }
}