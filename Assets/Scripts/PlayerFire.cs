using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerFire : MonoBehaviour {

    private ObjectPool bulletPool;

    public ObjectPool explosionPool;

    [Header("Bone used to look at target enemy")]
    public Transform weaponBone;
    public Transform playerLookDirection;

    public ThirdPersonCharacterMovement thirdPersonCharacterMovement; //Used for knockback effects

    [Header("Lazer for lazer weapons")]
    public Lazer lazer;

    private bool readyToFire = false;

    private float bulletToFireSpeed = 40f;
    private float bulletToFFireScale = 1f;
    private int bulletToFireDamage = 5;
    private bool bulletToFireExplosive = false;

    private float fireRate = 1f;
    private float timeSinceLastFire = 0f;
    private float shotAngleOffset = 2f;

    private Bullet currentBullet;

    public LockOn lockOn;

    public GameObject lookObject;

    private enum FireType{Bullet, Lazer};
    private FireType fireType;

    private int weaponIndex = 0;

    public PlayerSounds playerSounds;

    Transform[] childbones;

    private Hoverboard hoverboard;

    private void Awake(){
        childbones = weaponBone.GetComponentsInChildren<Transform>();
    }

    private void Start()
    {
        bulletPool = GameManager.Instance.bulletPool;
        hoverboard = FindObjectOfType<Hoverboard>();
    }

    private void Update()
    {
        if (GameManager.Instance.GetIsPaused())
        {
            return;
        }

        ChangeWeapon(GameManager.Instance.currentWeaponIndex);

        timeSinceLastFire += Time.deltaTime;
    }

    public void ChangeWeapon(int newWeaponIndex){
        weaponIndex = newWeaponIndex;

        AssignWeaponValues(weaponIndex);
    }

    private void AssignWeaponValues(int currentWeaponIndex){
        switch(currentWeaponIndex){
            case 0 :
            bulletToFFireScale = 0.7f;
            bulletToFireDamage = 5;
            bulletToFireExplosive = false;
            bulletToFireSpeed = 75f;
            fireRate = 0.15f;
            shotAngleOffset = 2f;
            fireType = FireType.Bullet;
            break;
            case 1 :
            bulletToFireDamage = 25;
            bulletToFireExplosive = false;
            fireRate = 1.4f;
            fireType = FireType.Lazer;
            break;
            case 2 :
            bulletToFireDamage = 60;
            bulletToFireExplosive = true;
            bulletToFFireScale = 1.6f;
            bulletToFireSpeed = 50f;
            fireRate = 0.9f;
            shotAngleOffset = 0f;
            fireType = FireType.Bullet;
            break;
            case 3 : //Rapid fire
            bulletToFireDamage = 3;
            bulletToFireExplosive = false;
            bulletToFFireScale = 0.4f;
            bulletToFireSpeed = 180f;
            fireRate = 0.06f;
            shotAngleOffset = 1f;
            fireType = FireType.Bullet;
            break;
            case 4 :
            bulletToFireDamage = 200;
            bulletToFireExplosive = false;
            fireRate = 1.4f;
            fireType = FireType.Lazer;
            break;
        }
    }

    private Vector3 armRotateOffset = new Vector3(90f, -90f, 0f);
    [HideInInspector]public Vector3 bulletTargetPosition;
    private const float SET_DOWN_ARM_TIME = 0.25f;
    private bool armLifted = false;
    private void LateUpdate(){
        if (GameManager.Instance.GetIsPaused())
        {
            return;
        }

        if (GameManager.Instance.GetPlayerInputManager().GetFireHeld())
        {
            readyToFire = true;
        }

        //handle all weapon arm rotation in here
        if (readyToFire || timeSinceLastFire < SET_DOWN_ARM_TIME)
        {
            if (hoverboard.IsBeingUsed())
            {
                float angle = Vector3.Angle(GameManager.Instance.thirdPersonCharacterMovement.thirdPersonPlayerCamera.transform.forward, GameManager.Instance.thirdPersonCharacterMovement.characterModel.transform.forward);
                if(angle > 70f)
                {
                    return;
                }
            }

            armLifted = true;

            //Arm point towards target
            if (lockOn.isLockedOn && lockOn.target != null)
            {
                bulletTargetPosition = lockOn.target.position;
            }
            else
            {
                bulletTargetPosition = Camera.main.transform.position + (Camera.main.transform.forward * 100f);
            }

            weaponBone.transform.LookAt(bulletTargetPosition);
            weaponBone.transform.Rotate(new Vector3(armRotateOffset.x, 0f, 0f));
            weaponBone.transform.Rotate(new Vector3(0f, armRotateOffset.y, 0f));
            weaponBone.transform.Rotate(new Vector3(0f, 0f, armRotateOffset.z));

            //bulletTargetPosition = weaponBone.transform.forward * 500f;

            for (int i = 0; i < childbones.Length; i++)
            {
                if (childbones[i] && childbones[i] != weaponBone)
                {
                    childbones[i].transform.LookAt(bulletTargetPosition);
                    childbones[i].transform.Rotate(new Vector3(armRotateOffset.x, 0f, 0f));
                    childbones[i].transform.Rotate(new Vector3(0f, armRotateOffset.y, 0f));
                    childbones[i].transform.Rotate(new Vector3(0f, 0f, armRotateOffset.z));
                }
            }
        } else
        {
            armLifted = false;
        }

        if (readyToFire)
        {
            Fire();
            readyToFire = false;
        }
    }

    public void Fire()
    {
        if(!thirdPersonCharacterMovement.hasControlOverCharacter)
        {
            return;
        }

        if(timeSinceLastFire >= fireRate)
        {
            if (fireType == FireType.Lazer)
            {
                lazer.EnableAudio();

                currentBullet = null;
                lazer.lerpMultiplier = 8f - fireRate;
                Vector3 endpoint = lockOn.GetMostRecentLockonLocation();
                CreateLazer(this.transform.position, endpoint);

                timeSinceLastFire = 0f;
            }

            if (fireType == FireType.Bullet)
            {
                currentBullet = bulletPool.GetObject().GetComponent<Bullet>();
                currentBullet.isEnemyBullet = false;

                currentBullet.speed = this.bulletToFireSpeed;
                currentBullet.scale = this.bulletToFFireScale;
                currentBullet.damageOutput = bulletToFireDamage;
                currentBullet.isExplosive = bulletToFireExplosive;

                currentBullet.transform.position = this.transform.position;
                currentBullet.trailRenderer.Clear();

                if (lockOn.isLockedOn && lockOn.target != null)
                {
                    currentBullet.transform.LookAt(lockOn.target);
                }
                else
                {
                    currentBullet.transform.LookAt(this.transform.position + Camera.main.transform.forward * 500f);
                }

                currentBullet.transform.Rotate(Random.Range(-shotAngleOffset, shotAngleOffset), Random.Range(-shotAngleOffset, shotAngleOffset), Random.Range(-shotAngleOffset, shotAngleOffset));

                currentBullet.destructionParticles.Stop();

                currentBullet.GetComponent<BulletSound>().PlaySound(weaponIndex);

                timeSinceLastFire = 0f;
            }
        }
    }

    public bool GetArmLifted()
    {
        return armLifted;
    }

    private void CreateLazer(Vector3 origin, Vector3 end)
    {
        lazer.lineRenderer.widthMultiplier = 0.85f;

        lazer.lineRenderer.SetPosition(0, origin);
        lazer.lineRenderer.SetPosition(1, end);

        thirdPersonCharacterMovement.dashVector = ((-thirdPersonCharacterMovement.characterModel.transform.forward) * 32f) + ((thirdPersonCharacterMovement.characterModel.transform.up) * 15f);

        RaycastHit[] hitObjects = Physics.RaycastAll(origin, this.transform.up);

        for(int i = 0; i < hitObjects.Length; i++){
            if(hitObjects[i].transform)
            {
                if(hitObjects[i].transform.GetComponent<Enemy>())
                hitObjects[i].transform.GetComponent<Enemy>().TakeDamage(bulletToFireDamage, hitObjects[i].transform.GetComponent<Enemy>().transform.position + Vector3.up + (-this.transform.up * 2f));

                if(hitObjects[i].transform.GetComponent<EnemyHurtBox>()){
                hitObjects[i].transform.GetComponent<EnemyHurtBox>().enemyToHurt.TakeDamage(bulletToFireDamage, hitObjects[i].transform.GetComponent<EnemyHurtBox>().enemyToHurt.transform.position + (this.transform.forward * 5f));
                }

                if(weaponIndex == 4)
                {
                    Explosion currentExplosion = explosionPool.GetObject().GetComponent<Explosion>();

                    currentExplosion.transform.position = hitObjects[i].point;
                    currentExplosion.transform.localScale = Vector3.one * 4f;
                    currentExplosion.Explode();
                    thirdPersonCharacterMovement.dashVector -= thirdPersonCharacterMovement.characterModel.transform.forward;
                }
            }
        }
    }
}
