using UnityEngine;

public class DecalManager : MonoBehaviour
{
    private void OnEnable()
    {
        OnDied.onDied += spawnDecal;
    }

    private void OnDisable()
    {
        OnDied.onDied -= spawnDecal;
    }

    void spawnDecal()
    {
        DecalPool.SharedInstance.getPooledObject();
    }
}
