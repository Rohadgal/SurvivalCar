using UnityEngine;

public class OnDied : MonoBehaviour
{
    public delegate void ZombieDied();

    public static event ZombieDied onDied;

    [SerializeField] private GameObject carTarget;
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Vector3 targetPos = carTarget.transform.position;
            if (Vector3.Distance(targetPos, this.transform.position) <= 20f)
            {
                RaycastHit hit;
                if (Physics.Raycast(this.transform.position, targetPos - this.transform.position, out hit))
                {
                    if (hit.transform.gameObject.CompareTag("Player") )
                    {
                        DecalPool.SharedInstance.tempNormal = hit.normal;
                        DecalPool.SharedInstance.tempPos = hit.point;
                        onDied();
                    }
                }
            }
        }
    }
}
    

