using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling : MonoBehaviour
{
    public static ObjectPooling instance;

    public List<GameObject> objectPool = new List<GameObject>();

    // Start is called before the first frame update
    private void Awake()
    {
        if (instance == null) instance = this;
    }

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject getPooledObject()
    {
        for (int i = 0; i < objectPool.Count; i++)
        {
            if (!objectPool[i].activeInHierarchy) return objectPool[i];
        }

        return null;
    }
}
