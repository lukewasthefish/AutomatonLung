using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour {

    public ObjectPool[] enemiesList;

    public int maxEnemiesActive = 4; //How many enemies can exist at the same time?

    [HideInInspector] public int currentEnemiesActive = 0; //This number will increment as EnemySpawnPoints are encountered and decrement when enemies are far away or are defeated.

    public ParticleSystem enemyDestroyedParticles;

    [HideInInspector] public ObjectPool enemyParticlesPool;

    [HideInInspector]public List<Transform> enemySpawnPointPositions = new List<Transform>();

    public float spawnRange = 45f;

    private void Awake()
    {
        GameManager.Instance.enemySpawnManager = this;

        for(int i = 0; i < enemiesList.Length; i++){
            if(enemiesList[i].objectToPool){
                enemiesList[i].gameObject.SetActive(true);
            }
        }

        enemyParticlesPool = this.gameObject.AddComponent<ObjectPool>();

        //Can't use a constructor with Addcomponent so the following 2 lines are what I would have liked to represent in a constructor for ObjectPool
        enemyParticlesPool.objectToPool = enemyDestroyedParticles.gameObject;
        enemyParticlesPool.numberOfObjectInstances = maxEnemiesActive;
    }

    public GameObject EnemyToSpawn(int enemyIndex)
    {
        if(currentEnemiesActive >= maxEnemiesActive)
        {
            return null;
        }

        GameObject go = enemiesList[enemyIndex].GetObject();

        if(go){
            return go;
        } else {
            return null;
        }
    }
}