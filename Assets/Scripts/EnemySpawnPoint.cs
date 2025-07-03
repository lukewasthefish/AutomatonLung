using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The EnemySpawnPoint will spawn an enemy at its position when the player enters its range.
/// It will trigger once and get an object from an objectPool to set to this position and activate.
/// When the player exits this EnemySpawnPoint range distance there is no need to despawn the spawned enemy,
/// and that would in fact cause more problems with inconsistent gameplay.
/// </summary>
public class EnemySpawnPoint : MonoBehaviour {

    [Header("Get enemy by index")]
    public int enemyIndex = 0;

    [HideInInspector]public bool hasBeenTriggered = false;

    private float thisSpawnRange;

    private void Start(){
        RegisterWithEnemySpawnManager();
    }

    private void RegisterWithEnemySpawnManager(){
        if(GameManager.Instance.enemySpawnManager)
            GameManager.Instance.enemySpawnManager.enemySpawnPointPositions.Add(this.transform);
    }

    public void TrySpawnEnemy()
    {
        if(!GameManager.Instance.enemySpawnManager || !GameManager.Instance.thirdPersonCharacterMovement){
            return;
        }

        if(GameManager.Instance.enemySpawnManager.currentEnemiesActive >= GameManager.Instance.enemySpawnManager.maxEnemiesActive){
            return;
        }

        thisSpawnRange = GameManager.Instance.enemySpawnManager.spawnRange;

        float distance = Vector3.Distance(this.transform.position, GameManager.Instance.thirdPersonCharacterMovement.transform.position);
        //Debug.DrawLine(this.transform.position, playerTranform.position, Color.cyan);

        if(distance > thisSpawnRange){
            return;
        }

        if (!hasBeenTriggered && distance > 5f)
        {
            //Is our spawn point position within the cameras view area?
            Vector3 worldToViewportPoint = Camera.main.WorldToViewportPoint(this.transform.position);
            if(!(worldToViewportPoint.x > 0 && worldToViewportPoint.x < 1 && worldToViewportPoint.y > 0 && worldToViewportPoint.y < 1 && worldToViewportPoint.z >= 0)){
                return;
            }

            //Are there any walls in the way of our direct line to the player?
            if(Physics.Linecast(this.transform.position + Vector3.up, GameManager.Instance.thirdPersonCharacterMovement.transform.position + Vector3.up)){
                return;
            }

            //Spawn enemy
            GameObject currentEnemy = GameManager.Instance.enemySpawnManager.EnemyToSpawn(enemyIndex);
            if(currentEnemy == null)
            {
                return;
            } else
            {
                if(currentEnemy.GetComponent<Enemy>()){
                    currentEnemy.GetComponent<Enemy>().Revive();
                    currentEnemy.GetComponent<Enemy>().enemySpawnPoint = this;

                    currentEnemy.gameObject.SetActive(true);

                    currentEnemy.transform.position = this.transform.position + Vector3.up;

                    GameManager.Instance.enemySpawnManager.currentEnemiesActive++;

                    this.hasBeenTriggered = true;
                }
            }
        }
    }
}
