using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public float timer = 0;
    public float spawnTime;

    // Start is called before the first frame update
    void Start()
    {
        ObjectPooling pooledEnemies = ObjectPooling.instance;
        for (int i= 0; i < pooledEnemies.objectPool.Count; i++)
        {
            pooledEnemies.objectPool[i].SetActive(true);
        }
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer >= 10)
        {
            spawn();
        }
    }

    void spawn()
    {
        GameObject enemy = ObjectPooling.instance.getPooledObject();
        if (enemy != null)
        {
            if(enemy.gameObject.tag == "Bear")
            {
                Bear bear = enemy.GetComponent<Bear>();
                bear.revive();
            }

            enemy.SetActive(true);
        }
    }
}
