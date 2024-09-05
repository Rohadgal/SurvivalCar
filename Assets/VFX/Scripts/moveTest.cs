using UnityEngine;

public class moveTest : MonoBehaviour
{
    public float speed = 15f;
    [SerializeField] private GameObject target;
    // Start is called before the first frame update
    void Start()
    {
        //this.transform.LookAt(target.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (target)
        {
            // Vector3 targetPos = target.transform.position - this.transform.position;
            // targetPos = targetPos.normalized;
            // this.transform.position += targetPos * Time.deltaTime;
            this.transform.position =
                Vector3.MoveTowards(this.transform.position, target.transform.position, speed * Time.deltaTime);
        }
    }
    

}
