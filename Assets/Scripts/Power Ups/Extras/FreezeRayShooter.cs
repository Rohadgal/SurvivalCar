using UnityEngine;
using UnityEngine.VFX;

public class FreezeRayShooter : MonoBehaviour {
    [SerializeField] private VisualEffect rayEffect;

    // TODO: Shoot a raycast to freeze enemies getting hit.
    public void shoot() {
        rayEffect.Play();
    }
}