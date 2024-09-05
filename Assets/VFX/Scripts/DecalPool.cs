using System.Collections.Generic;
using UnityEngine;

public class DecalPool : MonoBehaviour
{
    public static DecalPool SharedInstance;
    public List<GameObject> pooledObjects;
    public GameObject decalToPool;
    public int amountToPool;
    public Vector3 tempPos { get; set; }
    public Vector3 tempNormal { get; set; }
    public int index { get; set; }
    

    private void Awake()
    {
        SharedInstance = this;
    }

    private void Start()
    {
        pooledObjects = new List<GameObject>();
        GameObject temp;
        for (int i = 0; i < amountToPool; i++)
        {
            temp = Instantiate(decalToPool, this.transform, true);
            temp.SetActive(false);
            pooledObjects.Add(temp);
        }
    }

    public void getPooledObject()
    {
        if (index >= amountToPool)
        {
            index = 0;
        }
        
        if (!pooledObjects[index].activeInHierarchy)
        {
            pooledObjects[index].transform.rotation = Quaternion.LookRotation(tempNormal);
            pooledObjects[index].transform.position = tempPos + (-tempNormal * 0.15f);
            GameObject temp = pooledObjects[index++];
            temp.SetActive(true);
            return;
        }
     
    }
}
