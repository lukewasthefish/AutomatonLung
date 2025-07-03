using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnPointActivator : MonoBehaviour {

	private float maxTargetDistance;

	private void Awake(){
		InvokeRepeating("SlowUpdate", 2f, 0.1f);
	}

	private void SlowUpdate(){
		Search();
	}

	private void Search(){
		if(!GameManager.Instance || !GameManager.Instance.enemySpawnManager){
			return;
		}

		maxTargetDistance = GameManager.Instance.enemySpawnManager.spawnRange;

		for(int i = 0; i < GameManager.Instance.enemySpawnManager.enemySpawnPointPositions.Count; i++){
			if(GameManager.Instance.enemySpawnManager.enemySpawnPointPositions[i] && Vector3.Distance(this.transform.position, GameManager.Instance.enemySpawnManager.enemySpawnPointPositions[i].position) <= maxTargetDistance){
				GameManager.Instance.enemySpawnManager.enemySpawnPointPositions[i].GetComponent<EnemySpawnPoint>().TrySpawnEnemy();
			}
		}
	}
}
