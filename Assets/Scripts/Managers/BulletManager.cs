using System.Collections.Generic;
using System.Linq;
using AudioSystem;
using UnityEngine;

[RequireComponent(typeof(BulletFactory))]
public class BulletManager : MonoBehaviour {
    public static BulletManager s_instance;

    [SerializeField] private GameObject bulletPrefab;

    private readonly List<BulletMovement> m_pool = new List<BulletMovement>();
    
    private void Awake() {
        if (FindObjectOfType<BulletManager>() != null &&
            FindObjectOfType<BulletManager>().gameObject != gameObject) {
            Destroy(gameObject);
            return;
        }
        s_instance = this;
    }

    public BulletMovement spawnBullet(Vector3 t_spawnPosition) {
        foreach (BulletMovement bullet in m_pool.Where(t_bullet => !t_bullet.gameObject.activeInHierarchy)) {
            bullet.transform.position = t_spawnPosition;
            bullet.gameObject.SetActive(true);
            return bullet;
        }
        BulletMovement newBullet = BulletFactory.s_instance.spawnBullet(bulletPrefab);
        newBullet.transform.position = t_spawnPosition;
        newBullet.transform.SetParent(transform);
        m_pool.Add(newBullet);
        return newBullet;
    }
}