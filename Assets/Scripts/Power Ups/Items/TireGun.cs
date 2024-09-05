using AudioSystem;
using UnityEngine;

/// <summary>
/// Represents a tire gun power-up in the game, which shoots tires at intervals.
/// </summary>
[CreateAssetMenu(fileName = "Tire Gun", menuName = "CarsSurvivor/Tire Gun")]
public class TireGun : Item, IPowerUp {
    [SerializeField] private GameObject tirePrefab;
    [SerializeField] private float shootIntervalInSeconds = 2f;
    [SerializeField] private float damage = 50f;
    [SerializeField] private float throwForce = 15f;
    [SerializeField] private SoundData shootSoundData;
    
    private float m_countdown = 0;
    private Transform m_spawnPosition;
    private readonly Color m_tileColor = new Color(0.023f, 0.682f, 0.832f);

    /// <summary>
    /// Initializes the tire gun, setting the spawn position and resetting the countdown.
    /// </summary>
    /// <param name="t_parent">The parent GameObject from which the tire will be shot.</param>
    public void initialize(GameObject t_parent) {
        m_spawnPosition = t_parent.transform.Find("TopSpawnPoint");
        m_countdown = shootIntervalInSeconds;
    }

    /// <summary>
    /// Updates the tire gun, checking if it's time to shoot a tire and handling the shooting process.
    /// </summary>
    public void update() {
        m_countdown -= Time.deltaTime;
        if (!(m_countdown <= 0)) {
            return;
        }
        // Play the shoot sound with a random pitch
        SoundManager.Instance.createSoundBuilder().withRandomPitch().play(shootSoundData);
        // Create and shoot the tire
        TireMovement tempTire = TireFactory.Instance.creteTire(tirePrefab);
        tempTire.setDamage(damage);
        // Generate a random direction for the tire to be thrown
        float randomX = Mathf.Cos(Random.Range(0f, 360f));
        float randomZ = Mathf.Cos(Random.Range(0f, 360f));
        Vector3 randomDirection = new Vector3(randomX, 0, randomZ);
        // Set the tire's position and apply force to shoot it
        tempTire.transform.position = m_spawnPosition.position;
        tempTire.GetComponent<Rigidbody>().velocity = Vector3.zero;
        tempTire.GetComponent<Rigidbody>().AddForce(randomDirection * throwForce, ForceMode.Impulse);
        // Reset the countdown
        m_countdown = shootIntervalInSeconds;
    }

    /// <summary>
    /// Exits the tire gun, currently not implemented.
    /// </summary>
    public void exit() {
        throw new System.NotImplementedException();
    }

    /// <summary>
    /// Gets the color associated with the tire gun item.
    /// </summary>
    /// <returns>The color representing the tire gun.</returns>
    public override Color getColor() {
        return m_tileColor;
    }
}
