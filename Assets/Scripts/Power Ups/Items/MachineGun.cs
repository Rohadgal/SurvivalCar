using AudioSystem;
using UnityEngine;

/// <summary>
/// Represents a machine gun item in the game that can be used as a power-up.
/// </summary>
[CreateAssetMenu(fileName = "Machine Gun", menuName = "CarsSurvivor/Machine Gun")]
public class MachineGun : Item, IPowerUp {
    [SerializeField] private float shootIntervalInSeconds = 2f;
    [SerializeField] private float damage = 20f;
    [SerializeField] private SoundData shootSoundData;

    private float m_countdown = 0;
    private Transform[] m_spawnPositions;
    private readonly Color m_tileColor = new Color(0.972f, 0.866f, 0.423f);

    /// <summary>
    /// Initializes the machine gun power-up.
    /// </summary>
    public void initialize(GameObject t_parent) {
        Transform tempTransform = t_parent.transform.Find("GunSpawnPositions");
        m_spawnPositions = new Transform[tempTransform.childCount];
        int i = 0;
        foreach (Transform child in tempTransform) {
            m_spawnPositions[i] = child.transform;
            i++;
        }
    }

    /// <summary>
    /// Updates the machine gun power-up state.
    /// </summary>
    public void update() {
        m_countdown -= Time.deltaTime;
        if (!(m_countdown <= 0)) {
            return;
        }
        SoundManager.Instance.createSoundBuilder().withRandomPitch().play(shootSoundData);
        Vector3 spawnPosition = Random.value > .5 ? m_spawnPositions[0].position : m_spawnPositions[1].position;
        BulletMovement tempBullet = BulletManager.s_instance.spawnBullet(spawnPosition);
        tempBullet.updateDamage(damage);
        m_countdown = shootIntervalInSeconds;
    }

    /// <summary>
    /// Cleans up and exits the machine gun power-up.
    /// </summary>
    public void exit() {
    }
    
    public override Color getColor() {
        return m_tileColor;
    }
}