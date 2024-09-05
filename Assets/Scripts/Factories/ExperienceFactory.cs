using UnityEngine;

public class ExperienceFactory : MonoBehaviour {
    public static ExperienceFactory s_instance;
    
    private void Awake() {
        if (FindObjectOfType<ExperienceFactory>() != null &&
            FindObjectOfType<ExperienceFactory>().gameObject != gameObject) {
            Destroy(gameObject);
            return;
        }
        s_instance = this;
    }

    public GameObject spawnExperience(GameObject t_experiencePrefab) {
        return Instantiate(t_experiencePrefab);
    }
}