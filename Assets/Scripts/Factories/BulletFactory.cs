using UnityEngine;

public class BulletFactory : MonoBehaviour {
    public static BulletFactory s_instance;
    
    private void Awake() {
        if (FindObjectOfType<BulletFactory>() != null &&
            FindObjectOfType<BulletFactory>().gameObject != gameObject) {
            Destroy(gameObject);
            return;
        }
        s_instance = this;
    }

    public BulletMovement spawnBullet(GameObject t_prefabToSpawn) {
        return Instantiate(t_prefabToSpawn).GetComponent<BulletMovement>();
    }
}