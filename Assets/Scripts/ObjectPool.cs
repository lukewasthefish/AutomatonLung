using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour {

    public GameObject objectToPool;

    public int numberOfObjectInstances = 20;

    private List<GameObject> pool;

    private bool isPopulated = false;

    private void Awake()
    {
        isPopulated = false;
        pool = new List<GameObject>();
    }

    public GameObject GetObject()
    {
        if (pool == null || pool.Count <= 1)
        {
            Populate();
        }

        for(int i = 0; i < pool.Count; i++){
            if(pool[i].activeSelf == false)
            {
                pool[i].SetActive(true);

                return pool[i];
            }
        }

        int poolIndex = Mathf.RoundToInt(Random.Range(0, pool.Count));

        if(poolIndex < pool.Count && poolIndex >= 0 && pool[poolIndex]){

        return pool[poolIndex];
        }

        return pool[0];
    }

    public void Clear(){
        for(int i = 0; i < pool.Count; i++){
            Destroy(pool[i]);
        }

        isPopulated = false;

        pool.Clear();
    }

    private void Populate()
    {
        if(isPopulated){
            return;
        }

        pool = new List<GameObject>();

        for (int i = 0; i < numberOfObjectInstances; i++)
        {
            if(!objectToPool){
                continue;
            }

            GameObject currentObjectInstance = Instantiate(objectToPool);

            pool.Add(currentObjectInstance);
            currentObjectInstance.SetActive(false);

            if(this != null && this.transform != null)
            currentObjectInstance.transform.parent = this.transform;
        }

        isPopulated = true;
    }

    public List<GameObject> GetPooledObjectList(){
        return pool;
    }
}
