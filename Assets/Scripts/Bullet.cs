using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A player or enemy bullet typically drawn from an ObjectPool of pre-generated bullets.
/// </summary>
public class Bullet : MonoBehaviour {
    [HideInInspector]public float speed = 40f;
    private float initialSpeed;
    [HideInInspector]public float scale = 1f;
    private float initialScaleFloat;
    private Vector3 initialScale;


    public Renderer thisRenderer;
    public int damageOutput = 5;
    private int initialDamageOutput;
    private PlayerFire umaFire;

    [HideInInspector]public bool isExplosive = false;

    public ParticleSystem destructionParticles;

    [HideInInspector]public bool isEnemyBullet = false;

    [HideInInspector]
    public TrailRenderer trailRenderer;

    private void Awake(){
        initialScale = this.transform.localScale;
        initialDamageOutput = damageOutput;
        initialScaleFloat = scale;
        initialSpeed = speed;

        trailRenderer = this.GetComponent<TrailRenderer>();
    }

    private void OnEnable()
    {
        trailRenderer.Clear();
    }

    private void OnDisable()
    {
        trailRenderer.Clear();
    }

    private void FixedUpdate()
    {
        transform.localScale = initialScale * scale;
        transform.position += transform.forward * (speed/2f) * Time.deltaTime;

        //Distance-based bullet removal
        if(Camera.main && Vector3.Distance(this.transform.position, Camera.main.transform.position) > 50f)
        {
            trailRenderer.Clear();
            this.ReturnBulletToPool();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("IgnoreBullets") || other.GetComponent<PlayerHurtBox>() || other.GetComponent<Bullet>()){
            return; //Pass through these objects
        }

        trailRenderer.Clear();

        if (isEnemyBullet){
            EnemyBulletCollision(other);
        } else {
            PlayerBulletCollision(other);
        }

        BreakableBlock breakableBlock = other.GetComponent<BreakableBlock>();
        if(breakableBlock){
            EndBulletLife();
            breakableBlock.Break();
            return;
        }

        EventSwitch eventSwitch = other.GetComponent<EventSwitch>();
        if(eventSwitch){
            EndBulletLife();
            Debug.Log("Got event switch");
            eventSwitch.Trigger();
            return;
        }

        if(other.tag == "Untagged" && !other.GetComponent<Enemy>()){
            EndBulletLife();
        }
    }

    private bool PlayerBulletCollision(Collider other){
        Enemy hitEnemy = other.gameObject.GetComponent<Enemy>();
        if (hitEnemy)
        {
            EndBulletLife();
            hitEnemy.TakeDamage(damageOutput, this.transform.position - this.transform.forward);
            return true;
        }

        EnemyHurtBox ehr = other.GetComponent<EnemyHurtBox>();
        if(ehr){
            EndBulletLife();
            ehr.enemyToHurt.TakeDamage(damageOutput, this.transform.position - this.transform.forward);
            return true;
        }

        return false;
    }

    private bool EnemyBulletCollision(Collider other){
        ThirdPersonCharacterMovement thirdPersonCharacterMovement = other.gameObject.GetComponent<ThirdPersonCharacterMovement>();
        if(thirdPersonCharacterMovement != null && !thirdPersonCharacterMovement.GetComponent<PlayerCombat>().dead)
        {
            EndBulletLife();

            //damage player
            PlayerCombat pc = thirdPersonCharacterMovement.GetComponent<PlayerCombat>();
            if(pc)
                pc.TakeDamage(damageOutput, this.transform.forward);
            return true;
        }

        return false;
    }

    public void EndBulletLife()
    {
        trailRenderer.Clear();

        if (!umaFire && GameManager.Instance.thirdPersonCharacterMovement && GameManager.Instance.thirdPersonCharacterMovement.GetComponent<PlayerCombat>()){
            umaFire = GameManager.Instance.thirdPersonCharacterMovement.GetComponent<PlayerCombat>().GetUmaFire();
        }

        this.GetComponent<Collider>().enabled = false;

        thisRenderer.enabled = false;

        if(umaFire && isExplosive){
            Explosion currentExplosion = umaFire.explosionPool.GetObject().GetComponent<Explosion>();
            currentExplosion.transform.position = this.transform.position;
            currentExplosion.transform.localScale = Vector3.one * 2f;
            currentExplosion.Explode();
        }

        destructionParticles.Play();
        destructionParticles.transform.parent = null;

        Invoke("ReturnBulletToPool", 0.25f);
        CancelInvoke("EndBulletLife");
    }

    private void ReturnBulletToPool()
    {
        trailRenderer.Clear();

        damageOutput = initialDamageOutput;
        scale = initialScaleFloat;
        speed = initialSpeed;
        isExplosive = false;

        if(destructionParticles){
        destructionParticles.transform.position = transform.position;
        destructionParticles.transform.parent = this.transform;
        destructionParticles.Stop();
        }

        if(this.GetComponent<Collider>()){
        this.GetComponent<Collider>().enabled = true;
        }

        if(thisRenderer){
        thisRenderer.enabled = true;
        }

        this.gameObject.SetActive(false);
    }
}