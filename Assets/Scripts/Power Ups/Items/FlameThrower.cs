using UnityEngine;

[CreateAssetMenu(fileName = "Flamethrower", menuName = "CarsSurvivor/Flamethrower")]
public class FlameThrower : Item, IPowerUp {
    [SerializeField] private GameObject flameThrowerPrefab;
    [SerializeField] private float rotationAngle = 1;

    private FlameThrowerShooter m_flameThrowerShooter;
    private readonly Color m_tileColor = new Color(0.866f, 0.109f, 0.101f);

    public void initialize(GameObject t_parent) {
        Transform tempTransform = t_parent.transform.Find("TopSpawnPoint");
        m_flameThrowerShooter = Instantiate(flameThrowerPrefab).GetComponent<FlameThrowerShooter>();
        m_flameThrowerShooter.transform.SetParent(tempTransform);
        m_flameThrowerShooter.transform.localPosition = Vector3.zero; 
    }

    public void update() {
        Vector3 rotationToAdd = new Vector3(0, rotationAngle, 0);
        m_flameThrowerShooter.transform.Rotate(rotationToAdd);
    }

    public void exit() {
        
    }
    
    public override Color getColor() {
        return m_tileColor;
    }
}