using UnityEngine;

[CreateAssetMenu(fileName = "Freeze Ray", menuName = "CarsSurvivor/Freeze Ray")]
public class FreezeRay : Item, IPowerUp {
    [SerializeField] private GameObject beamPrefab;
    [SerializeField] private float shootIntervalInSeconds = 2f;
    [SerializeField] private float rotationAngle = 30f;

    private FreezeRayShooter m_freezeRayShooter;
    private float m_countdown;
    private readonly Color m_tileColor = new Color(0.933f, 0.529f, 0.458f);

    public void initialize(GameObject t_parent) {
        Transform tempTransform = t_parent.transform.Find("TopSpawnPoint");
        m_freezeRayShooter = Instantiate(beamPrefab).GetComponent<FreezeRayShooter>();
        m_freezeRayShooter.transform.SetParent(tempTransform);
        m_freezeRayShooter.transform.localPosition = Vector3.zero;
        m_countdown = shootIntervalInSeconds;
    }

    public void update() {
        m_countdown -= Time.deltaTime;
        if (!(m_countdown <= 0)) {
            return;
        }
        m_countdown = shootIntervalInSeconds;
        Vector3 rotationToAdd = new Vector3(0, rotationAngle, 0);
        m_freezeRayShooter.transform.Rotate(rotationToAdd);
        m_freezeRayShooter.shoot();
    }

    public void exit() {
        throw new System.NotImplementedException();
    }
    
    public override Color getColor() {
        return m_tileColor;
    }
}