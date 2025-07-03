using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonPlayerCamera : MonoBehaviour
{
    private Transform flattenedCamRotation; //Returns only the y rotation. like a 2D rotation.

    public Transform objectToFollow;

    //public UIJoystick camJoystick;

    private Vector3 targetPosition = Vector3.zero;
    private Vector3 currentLookLocation;
    private Vector3 destinationLookLocation;

    public enum CameraMode { standard, hoverBoard, death }

    public CameraMode cameraMode;

    //Zone system variables
    [HideInInspector] public Transform zoneCameraDestination;

    public LockOn lockOn;

    [Header("Camera Properties")]
    private float DistanceAway;//how far the camera is from the player.

    public float maxHeight = 120f;
    public float minHeight = -5f;

    public float minDistance = 1;
    public float maxDistance = 2;

    public float DistanceUp = -2;
    [HideInInspector] public float rotateAround;//the angle at which you will rotate the camera (on an axis)

    public float rotateAxisSpeed = 50f; //Speed at which to rotate around when using Player input
    private float heightOverPlayer = 2f;

    [Header("Player to follow")]
    public Transform target;                    //the target the camera follows

    [Header("Layer(s) to include")]
    public LayerMask CamOcclusion;                //the layers that will be affected by collision

    float cameraHeight = 55f;
    private Vector3 currentCamPosition;
    private Vector3 desiredCamPosition;

    private const float MOVEMENT_LERP = 64f;
    private const float LOCKON_CAMERAHEIGHT = 40f;
    private float currentMovementLerp = MOVEMENT_LERP;

    private const float ROTATION_LERP = 32f;
    private float currentRotationLerp = ROTATION_LERP;

    private bool useLerp = false;

    private Transform characterModel;

    [HideInInspector]public CameraShake cameraShake;

    public ThirdPersonCharacterMovement thirdPersonCharacterMovement;

    private float verticalInvertedCameraMultiplier = 1f; //-1 for inverted, 1 for regular
    private float horizontalInvertedCameraMultiplier = 1f;

    private PlayerFire playerFire;

    private void Awake(){
        playerFire = FindObjectOfType<PlayerFire>();
        cameraShake = this.GetComponent<CameraShake>();

        if (GameManager.Instance.invertCameraVertical)
        {
            verticalInvertedCameraMultiplier = -1f;
        }

        if (GameManager.Instance.invertCameraHorizontal)
        {
            horizontalInvertedCameraMultiplier = -1f;
        }
    }

    private void OccludeRay(ref Vector3 targetFollow)
    {
        //During lockon avoid wall clipping to prioritize smooth combat
        if (lockOn.isLockedOn)
        {
            return;
        }

        if(GameManager.Instance.thirdPersonCharacterMovement.GetComponent<ThirdPersonCharacterMovement>().GetMovementType() == ThirdPersonCharacterMovement.MovementType.Teleport)
        {
            return;
        }

        //linecast from your player (targetFollow) to your cameras mask (camMask) to find collisions.
        if (Physics.Linecast(targetFollow, currentCamPosition, out RaycastHit wallHit, CamOcclusion) /*Physics.SphereCast(targetPosition,0.5f, this.transform.forward, out wallHit, CamOcclusion)*/)
        {
            //the x and z coordinates are pushed away from the wall by hit.normal.
            //the y coordinate stays the same.
            currentCamPosition = new Vector3(wallHit.point.x + wallHit.normal.x * 0.05f, currentCamPosition.y, wallHit.point.z + wallHit.normal.z * 0.05f);

            if (!flattenedCamRotation)
            {
                // This gameobject will represent the transform of this camera on the XZ axis only. Useful for determining where character should face.
                flattenedCamRotation = new GameObject("FlattenedCamRotation").transform;
            }

            flattenedCamRotation.rotation = new Quaternion(0f, this.transform.rotation.y, 0f, this.transform.rotation.w);

            destinationLookLocation = target.position + (flattenedCamRotation.transform.forward);
        }
    }

    private readonly float camMovementDeadzone = 0.2f;
    private void ReceiveCameraAxisInput()
    {
        if (lockOn.isLockedOn || GameManager.Instance.GetIsPaused() || Time.timeSinceLevelLoad < 0.5f)
        {
            return;
        }

        float mouseSensitivityScalar = (GameManager.Instance.mouseSensitivity / 50f);

        if(mouseSensitivityScalar <= 0f)
        {
            mouseSensitivityScalar = 1f;
        }

        if(verticalInvertedCameraMultiplier == 0f)
        {
            verticalInvertedCameraMultiplier = 1f;
        }

        if(horizontalInvertedCameraMultiplier == 0f)
        {
            horizontalInvertedCameraMultiplier = 1f;
        }

        float cameraVertical = GameManager.Instance.GetPlayerInputManager().GetRightStick().y * verticalInvertedCameraMultiplier * mouseSensitivityScalar;
        float cameraHorizontal = GameManager.Instance.GetPlayerInputManager().GetRightStick().x * horizontalInvertedCameraMultiplier * mouseSensitivityScalar;
        //Joystick axis on camera movement horizontal
        if (Mathf.Abs(cameraHorizontal) > camMovementDeadzone)
        {
            rotateAround += rotateAxisSpeed * cameraHorizontal * Time.deltaTime;
        }

        //Vertical camera movement
        //Joystick axis vertical
        if (Mathf.Abs(cameraVertical) > camMovementDeadzone)
        {
            cameraHeight -= rotateAxisSpeed * cameraVertical * Time.deltaTime;

            if(cameraHeight > maxHeight)
            {
                cameraHeight = maxHeight;
            }

            if(cameraHeight < minHeight)
            {
                cameraHeight = minHeight;
            }
        }

        //Adjust camera height if exceeding desired range
        while (cameraHeight < minHeight)
        {
            cameraHeight += Time.deltaTime;
        }

        while (cameraHeight > maxHeight)
        {
            cameraHeight -= Time.deltaTime;
        }
    }

    private void PopulatePlayerRenderers()
    {
        playerRenderers.AddRange(FindObjectOfType<PlayerRenderers>().playerRenderers);
    }

    private Vector3 whileFiringOffset;
    private List<Renderer> playerRenderers = new List<Renderer>();
    private void CameraMovement()
    {
        if(GameManager.Instance.GetIsPaused())
        {
            return;
        }

        if(!characterModel){
            if(GameManager.Instance.thirdPersonCharacterMovement)
            characterModel = GameManager.Instance.thirdPersonCharacterMovement.GetComponent<ThirdPersonCharacterMovement>().characterModel.transform;
        }

        DistanceAway = Mathf.Clamp(DistanceAway, minDistance, maxDistance);

        //From original version of game, hides player if they get too close to camera
        //if(Vector3.Distance(this.transform.position, characterModel.transform.position + Vector3.up) < Camera.main.nearClipPlane + 0.1f){
        //    HidePlayerRenderers(playerRenderers);
        //} else if(thirdPersonCharacterMovement.hasControlOverCharacter) {
        //    ShowPlayerRenderers(playerRenderers);
        //}

        ReceiveCameraAxisInput();

        if (!target)
        {
            return;
        }

        destinationLookLocation = target.position + Vector3.up * 1.3f;

        if (objectToFollow)
        {
            if (targetPosition == Vector3.zero)
            {
                targetPosition = (objectToFollow.transform.position + Vector3.up + (-objectToFollow.transform.forward * 2f));
                transform.position = targetPosition;
            }
        }

        //Offset of the targets transform (Since the pivot point is usually at the feet).
        Vector3 targetOffset = new Vector3(target.position.x, (target.position.y + heightOverPlayer), target.position.z);
        Quaternion rotation = Quaternion.Euler(cameraHeight, rotateAround, 0f);
        Vector3 vectorMask = Vector3.one;
        Vector3 rotateVector = rotation * vectorMask;

        if (cameraMode == CameraMode.standard || cameraMode == CameraMode.hoverBoard)
        {
            Vector3 currentCamPosition = targetOffset + (Vector3.up * DistanceUp) - (rotateVector * DistanceAway);

            if(lockOn.target && lockOn.isLockedOn && characterModel)
            {
                desiredCamPosition = currentCamPosition + (characterModel.transform.right/1.25f) + (characterModel.transform.forward * 1.4f);
                cameraHeight = Mathf.Lerp(cameraHeight, LOCKON_CAMERAHEIGHT, 8f * Time.deltaTime);
                destinationLookLocation = lockOn.target.position;
            } else
            {
                desiredCamPosition = currentCamPosition;
            }

            Vector3 destinationWhileFiringOffset;
            if (playerFire.GetArmLifted())
            {
                destinationWhileFiringOffset = characterModel.transform.forward * -4.4f;
            } else
            {
                destinationWhileFiringOffset = Vector3.zero;
            }

            whileFiringOffset = Vector3.Lerp(whileFiringOffset, destinationWhileFiringOffset, 5f * Time.deltaTime);
        }

        if (lockOn.isLockedOn)
        {
            rotateAround = objectToFollow.transform.rotation.eulerAngles.y - 35f;
        }

        if (rotateAround > 360)
        {
            rotateAround = 0f;
        }
        else if (rotateAround < 0f)
        {
            rotateAround += 360f;
        }

        useLerp = lockOn.TimeSinceLockOn > 0f && lockOn.TimeSinceLockOn < 2f;

        if(useLerp)
        {
            if(currentMovementLerp < MOVEMENT_LERP)
            {
                currentMovementLerp += Time.deltaTime * MOVEMENT_LERP;
            }

            if(currentRotationLerp < ROTATION_LERP)
            {
                currentRotationLerp += Time.deltaTime * ROTATION_LERP;
            }

            currentCamPosition = Vector3.Lerp(currentCamPosition, desiredCamPosition, currentMovementLerp * Time.deltaTime);
            currentLookLocation = Vector3.Lerp(currentLookLocation, destinationLookLocation, currentRotationLerp * Time.deltaTime);
        } else
        {
            currentMovementLerp = 0f;
            currentRotationLerp = 0f;

            currentCamPosition = desiredCamPosition;
            currentLookLocation = destinationLookLocation;
        }

        OccludeRay(ref targetOffset);

        this.transform.position = currentCamPosition;
        this.transform.LookAt(currentLookLocation);
        this.transform.position = currentCamPosition;
        //Camera shake
        cameraShake.ReadShake();
    }

    private void LateUpdate()
    {
        if(cameraMode == CameraMode.death){
            this.transform.position += Vector3.up * Time.deltaTime * 5f;

            return;
        }

        CameraMovement();
    }

    private void HidePlayerRenderers(IEnumerable<Renderer> renderers)
    {
        foreach(Renderer r in renderers)
        {
            r.enabled = false;
        }
    }

    private void ShowPlayerRenderers(IEnumerable<Renderer> renderers)
    {
        foreach(Renderer r in renderers)
        {
            r.enabled = true;
        }
    }
}