using System.Collections.Generic;
using System.Linq;
using AudioSystem;
using UnityEngine;

public class TireFactory : PersistentSingleton<TireFactory> {
    private readonly List<TireMovement> m_pool = new List<TireMovement>();

    public TireMovement creteTire(GameObject t_prefab) {
        foreach (TireMovement bullet in m_pool.Where(t_bullet => !t_bullet.gameObject.activeInHierarchy)) {
            bullet.gameObject.SetActive(true);
            return bullet;
        }
        TireMovement newBullet = Instantiate(t_prefab).GetComponent<TireMovement>();
        newBullet.transform.SetParent(transform);
        m_pool.Add(newBullet);
        return newBullet;
    }
}