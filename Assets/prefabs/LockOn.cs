using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Place this one the bone which should use the lookat transform
/// </summary>
public class LockOn : MonoBehaviour {

    private AudioSource audioSource;

    [HideInInspector] public bool isLockedOn = false;

    [Header("Model is used for getting angle to target")]
    public Transform model;

    private readonly float maxTargetDistance = 50f;

    public Crosshair crosshair;

    private PlayerFire playerFire;

    [HideInInspector]public Transform target;
    [HideInInspector]public Vector3 targetPosition;

    private float angleToTarget; //Look angle from this object to target. NOT using bone angle to target.

    [HideInInspector]public bool lockOnButton = false;
    [HideInInspector] public bool initialFire = true; //Fire off the lockonbutton once on Start()

    List<Transform> potentialTargets = new List<Transform>();
    public float TimeSinceLockOn { get; private set; } = Mathf.Infinity;

    private Vector3 mostRecentLockonLocation;
    private bool lockOnButtonDownLastFrame = false;

    private void Awake()
    {
        playerFire = FindObjectOfType<PlayerFire>();
        audioSource = GetComponent<AudioSource>();
    }

    private Transform FindTarget()
    {
        Collider[] allColliders = Physics.OverlapSphere(this.transform.position, maxTargetDistance);
        potentialTargets.Clear();

        for(int i = 0; i < allColliders.Length; i++){
            if (IsWithinSight(allColliders[i].transform.position) && allColliders[i].GetComponent<Enemy>() != null)
            {
                if(allColliders[i].GetComponent<Boss>())
                {
                    allColliders[i].GetComponent<Boss>().isBoss = true;
                }

                if (!allColliders[i].GetComponent<Enemy>().isBoss)
                {
                    potentialTargets.Add(allColliders[i].transform);
                }

                if (allColliders[i].GetComponent<Enemy>().isBoss)
                {
                    potentialTargets.Add(allColliders[i].GetComponent<Boss>().lockOnLocation);
                }
            }
        }

        float nearestTargetDistance = Mathf.Infinity;
        Transform transformToReturn = null;

        for(int i = 0; i < potentialTargets.Count; i++)
        {
            float currentDistance = Vector3.Distance(potentialTargets[i].position, this.transform.position);

            if (currentDistance < nearestTargetDistance)
            {
                nearestTargetDistance = currentDistance;

                transformToReturn = potentialTargets[i];
            }
        }

        if(transformToReturn != null){
            audioSource.pitch = 0.64f;
            audioSource.volume = 0.45f;
            audioSource.Play();
        }

        return transformToReturn; //Will return null if no potential targets can be found
    }

    public Vector3 GetMostRecentLockonLocation()
    {
        return mostRecentLockonLocation;
    }

    private void Update()
    {
        if (GameManager.Instance.GetIsPaused())
        {
            return;
        }

        if (isLockedOn && target)
        {
            TimeSinceLockOn = 0f;
        } else
        {
            TimeSinceLockOn += Time.deltaTime;
        }
    }

    //Must run bone transforms on LateUpdate to override animation transformations
    private void LateUpdate()
    {
        if (GameManager.Instance.GetIsPaused())
        {
            return;
        }

        lockOnButton = GameManager.Instance.GetPlayerInputManager().GetLockOnHeld();

        if(lockOnButton && !lockOnButtonDownLastFrame)
        {
            target = FindTarget();
            lockOnButtonDownLastFrame = true;
        }

        mostRecentLockonLocation = LookAtLookDirection();

        if (lockOnButton || initialFire)
        {
            if (target != null && target.gameObject.activeSelf)
            {
                DoLockOn(target, true);
            } else
            {
                DisengageLockOn();
            }
        } else
        {
            isLockedOn = false;
            crosshair.HideCrosshair();
        }

        initialFire = false;

        if (!lockOnButton)
        {
            lockOnButtonDownLastFrame = false;
        }
    }

    private void DoLockOn(Transform currentTarget, bool useCrosshair)
    {
        isLockedOn = true;

        if (useCrosshair)
        {
            crosshair.transform.position = target.transform.position;
        }
        Vector3 targetDirection = target.transform.position - this.transform.position;
        angleToTarget = Vector3.Angle(targetDirection, model.transform.forward);

        if (/*IsWithinSight(target.position) &&*/ Vector3.Distance(this.transform.position, target.position) < maxTargetDistance)
        {
            this.transform.LookAt(target.position);

            if (!crosshair.CrossHairShowing() && useCrosshair)
                crosshair.ShowCrosshair();
        }
        else
        {
            isLockedOn = false;
            target = null;
        }
    }

    private void DisengageLockOn()
    {
        Vector3 lookPosition = model.transform.position + model.transform.forward * 5f;
        targetPosition = lookPosition;

        isLockedOn = true;

        this.transform.LookAt(lookPosition);

        target = null;
        crosshair.HideCrosshair();
    }

    Hoverboard hoverboard;
    /// <summary>
    /// Look at the point the camera is currently looking at. Useful for when looking at something at which desired to fire at but no desire to currently lock on.
    /// </summary>
    private Vector3 LookAtLookDirection()
    {
        if (!hoverboard)
        {
            hoverboard = FindObjectOfType<Hoverboard>();
        }

        Vector3 desiredLookPosition = Camera.main.transform.position + Camera.main.transform.forward * 500f;

        if (IsWithinSight(desiredLookPosition, 90f) && playerFire.GetArmLifted() && !hoverboard.IsBeingUsed())
        {
            this.transform.LookAt(desiredLookPosition);
        }

        return desiredLookPosition;
    }

    private  bool IsWithinSight(Vector3 currentTargetPosition, float angleLimit = 40f)
    {
        Vector3 targetDirection = currentTargetPosition - this.transform.position;
        angleToTarget = Vector3.Angle(targetDirection, Camera.main.transform.forward);

        if (angleToTarget > -angleLimit && angleToTarget < angleLimit)
        {
            return true;
        }

        return false;
    }
}