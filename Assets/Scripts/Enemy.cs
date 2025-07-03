using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An abstract class from which different enemy types are extended. Contains the basic behaviors that all enemies need to maintain such as dying and taking damage.
/// </summary>
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(MeshFlash))]
[RequireComponent(typeof(EnemySounds))]
public abstract class Enemy : MonoBehaviour {

    public int maxHealth = 100;
    protected int currentHealth = 1; //Initialize at one to prevent death on Awake()

    public float moveSpeed = 4f;
    public float maxFallSpeed = 50f;
    public float fallAcceleration = 20f;
    private float angleToTarget;

    protected float currentFallSpeed;

    //Components
    protected CharacterController characterController;
    private EnemySounds enemySounds;
    private Renderer thisRenderer;
    private MeshFlash meshFlash;

    //Vector3
    private Vector3 moveDirection;
    protected Vector3 baseMoveDirection; //Move direction caused by this Enemy. Does not included knockback or external forces.
    [HideInInspector] public Vector3 knockback;

    //Booleans
    public bool isExplodingEnemy = false;
    private bool variablesInitialized = false;
    protected bool defeated = false;
    [HideInInspector] public bool isBoss = false; //Assigned in constructor in Boss.cs

    //References
    public GameObject characterModel;

    [HideInInspector] public EnemySpawnPoint enemySpawnPoint; //Where did we spawn from? Assigned via the EnemySpawnPoint itself.

    public Enemy(CharacterController cc, MeshFlash mf)
    {
        cc = GetComponent<CharacterController>();
        mf = FindMeshFlash();

        characterController = cc;
        meshFlash = mf;
    }

    protected virtual void Awake(){
        characterController = GetComponent<CharacterController>();
    }

    protected virtual void Start(){
        if(!variablesInitialized)
        {
            characterController = GetComponent<CharacterController>();
            thisRenderer = GetComponent<Renderer>();
            meshFlash = FindMeshFlash();

            if (thisRenderer == null)
            {
                thisRenderer = GetComponentInChildren<Renderer>();
            }

            variablesInitialized = true;
        }
    }

    private void OnEnable()
    {
        enemySounds = GetComponent<EnemySounds>();
        currentHealth = maxHealth;
    }

    public void Revive()
    {
        currentHealth = maxHealth;

        knockback = Vector3.zero;

        defeated = false;

        if(meshFlash == null)
        {
            meshFlash = FindMeshFlash();
        }

        if(meshFlash)
            meshFlash.Flash(0.5f);
    }

    private MeshFlash FindMeshFlash()
    {
        MeshFlash meshFlashToReturn = this.GetComponent<MeshFlash>();

        if(meshFlashToReturn == null)
        {
            meshFlashToReturn = this.GetComponentInChildren<MeshFlash>(true);
        }

        return meshFlashToReturn;
    }

    protected virtual void Update()
    {
        if(characterController)
            characterController.Move(moveDirection * Time.deltaTime);

        if(currentFallSpeed < maxFallSpeed)
        {
            currentFallSpeed += fallAcceleration * Time.deltaTime;
        }

        moveDirection = (baseMoveDirection * moveSpeed) + (Vector3.down * currentFallSpeed) + (knockback*10f); // + movedirection vectors

        knockback = Vector3.Lerp(knockback, Vector3.zero, 5f * Time.deltaTime);
    }

    private void LateUpdate(){

        if(!GameManager.Instance.enemySpawnManager){
            return;
        }

        float distance = 1000f;

        //Loading enemies in and out logic; disabling renderers for optimization. Bosses won't use this because they are always loaded in.
        if (!GetComponent<Boss>())
        {
            if(!defeated && currentHealth <= 0){
                Die();
            }

            if(GameManager.Instance.thirdPersonCharacterMovement)
                distance = Vector3.Distance(this.transform.position, GameManager.Instance.thirdPersonCharacterMovement.transform.position);

            bool withingGameRange = distance < 50f;

            if (!withingGameRange && !GetComponent<Boss>())
            {
                // UnityEngine.Debug.Log(this.gameObject.name + " has exited the game range");

                if(enemySpawnPoint){
                GameManager.Instance.enemySpawnManager.currentEnemiesActive--;
                enemySpawnPoint.hasBeenTriggered = false;
                this.gameObject.SetActive(false);
                }
            }
        }
    }

    protected void Die()
    {
        if(enemySounds == null){
            enemySounds = GetComponent<EnemySounds>();
        }

        if (GameManager.Instance.enemySpawnManager != null)
        {
            GameManager.Instance.enemySpawnManager.currentEnemiesActive--;
            GameObject currentEnemyDefeatedParticles = GameManager.Instance.enemySpawnManager.enemyParticlesPool.GetObject();

            if(currentEnemyDefeatedParticles){
            currentEnemyDefeatedParticles.transform.position = this.transform.position;

            currentEnemyDefeatedParticles.gameObject.SetActive(true);

            currentEnemyDefeatedParticles.GetComponent<ParticleSystem>().Play();
            }
            defeated = true;
        }

        //if(enemySounds)
        //    enemySounds.PlayHurt();

        if(!GameManager.Instance.GetIsPaused())
        Time.timeScale = 0.3f;

        Invoke("ResetTimeScale", 0.06f);

        this.gameObject.SetActive(false);
    }

    private void ResetTimeScale()
    {
        if(!GameManager.Instance.GetIsPaused())
        Time.timeScale = 1f;
    }

    protected virtual void Fire(Vector3 targetPosition, float bulletMovementSpeed)
    {
        if (defeated || !isActiveAndEnabled) return;

        Bullet currentBullet = GameManager.Instance.bulletPool.GetObject().GetComponent<Bullet>();

        if(!currentBullet){
            return;
        }

        currentBullet.isEnemyBullet = true;

        currentBullet.transform.position = this.transform.position;

        currentBullet.transform.LookAt(targetPosition);
        currentBullet.trailRenderer.Clear();
        currentBullet.destructionParticles.Stop();

        currentBullet.speed = bulletMovementSpeed;

        currentBullet.GetComponent<BulletSound>().PlaySound(0);
    }

    public virtual void TakeDamage(int damageAmmount, Vector3 attackDirection)
    {
        //knockback += new Vector3(this.transform.position.x - attackDirection.x, 0f, this.transform.position.z - attackDirection.z);
		knockback += this.transform.position - attackDirection;

        // UnityEngine.Debug.Log(this.gameObject.name + " took " + damageAmmount + " damage." + " " + currentHealth + " health remaining.");
        currentHealth -= damageAmmount;

        if (meshFlash == null) meshFlash = FindMeshFlash();

        if(meshFlash && meshFlash.isActiveAndEnabled)
        meshFlash.Flash(0.3f);
    }

    /// <summary>
    /// Taking a Transform 'currentTarget' and a float 'sightRange' determines whether 'currentTarget' is within this Enemy range of sight.
    /// </summary>
    /// <param name="currentTarget"></param> What are we looking for in our line of sight?
    /// <param name="sightRange"></param> Range in degrees at which this Enemy can 'see' in either direction.
    /// <returns></returns>
    protected bool IsWithinSight(Transform currentTarget, float sightRange)
    {
        Vector3 targetDirection = currentTarget.transform.position - this.transform.position;
        if(characterModel != null)
        {
            angleToTarget = Vector3.Angle(targetDirection, characterModel.transform.forward);
        } else
        {
            angleToTarget = Vector3.Angle(targetDirection, this.transform.forward);
        }

        if (angleToTarget > -sightRange && angleToTarget < sightRange)
        {
            return true;
        }

        return false;
    }
}
