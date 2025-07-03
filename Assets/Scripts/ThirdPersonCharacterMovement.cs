using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class ThirdPersonCharacterMovement : MonoBehaviour {

    public enum MovementType { OnFoot, Hoverboard, Teleport };

    private MovementType movementType = MovementType.OnFoot;

    private CharacterController characterController;

    private PlayerSounds playerSounds;

    public Animator animator;
    public GameObject characterModel;
    public Renderer[] characterRenderers;
    public GameObject shadow;
    public GameObject lensFlare;

    public float maxSpeed = 7f;
    public float maxWaterWalkSpeed = 3f;
    public float jumpStrength = 20f;
    public float maxFallSpeed = 15f;
    public float fallAcceleration = 22f;
    public float accelerationLerpRate = 5f;

    private float currentFallSpeed;
    [HideInInspector] public float vertical;
    [HideInInspector] public float horizontal;
    [HideInInspector] public float speedMultiplier;
    [HideInInspector]public float currentSpeed;
    public float maxHoverBoardSpeed = 10f;
    public float dashStrength = 5f;
    public float dashTimerMax = 1.4f;
    private float dashTimer = 0.1f;
    private float deathYLevel; //Y position after which to kill player if they fall too far.
    private float previousYPosition = 0f;
    private float dashTime = 0.2f;
    private float currentMaxSpeed;
    [HideInInspector] public float hoverBoardForwardMotion = 0f;

    private Vector3 moveDirection;
    private Vector3 slopeVector;
    private Vector3 initialLensFlareSize;
    private Transform destinationTeleportDestination;
    [HideInInspector]public Vector3 dashVector; //Also used for knockback when taking damage from an enemy for example.

    private Transform cameraTransform;
	public Transform trueShadow;
    private GameObject flattenedCamera;

    [HideInInspector]public ThirdPersonPlayerCamera thirdPersonPlayerCamera;

    private bool hasJumped = false;
    private bool dead = false;
    [HideInInspector]public bool hasControlOverCharacter = true;
    [HideInInspector]public bool isGrounded = true;
    [HideInInspector]public bool justTeleported = false;
    private bool justFell = false; //has the player just barely started falling descent?

    public LockOn lockOn;

    public TrailRenderer dashTrail;
    public Transform dashBackPanel;

    [HideInInspector]public bool hoverBoardPressed;

    public ParticleSystem teleportParticles;

    private RaycastHit hit;
    private RaycastHit groundShadowRaycastHit;

    private Hoverboard hoverboard;
    private MovementType previousMovementType; //Used for teleport

    private Vector3 initialBackPanelScale;

    private PlayerFire playerFire;

    public void SetCurrentMaxSpeed(float newCurrentMaxSpeed)
    {
        currentMaxSpeed = newCurrentMaxSpeed;
    }

    private void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        if (!playerFire)
        {
            playerFire = FindObjectOfType<PlayerFire>();
        }

        currentMaxSpeed = maxSpeed;

        hoverboard = FindObjectOfType<Hoverboard>();

        hasControlOverCharacter = false;

        initialLensFlareSize = lensFlare.transform.localScale;
        lensFlare.SetActive(false);

        characterController = GetComponent<CharacterController>();
        cameraTransform = Camera.main.transform;
        flattenedCamera = new GameObject();
        flattenedCamera.transform.rotation = cameraTransform.rotation;

        currentFallSpeed = -3f;

        initialBackPanelScale = dashBackPanel.transform.localScale;

        thirdPersonPlayerCamera = cameraTransform.gameObject.GetComponent<ThirdPersonPlayerCamera>();
        GameManager.Instance.thirdPersonCharacterMovement = this;
        playerSounds = GetComponent<PlayerSounds>();
        Invoke("EnableControlOverCharacter", 0.3f);
    }

    public void EnableControlOverCharacter(){
        hasControlOverCharacter = true;
    }

    public void DisableControlOverCharacter(){
        hasControlOverCharacter = false;
    }

    public void ToggleControlOverCharacter(){
        hasControlOverCharacter = !hasControlOverCharacter;
    }

    private void Start(){
        deathYLevel = this.transform.position.y - 1000f;
    }

    private bool hasJustPressedLockOn = false;

    private void Update()
    {
        //Cheat debug autocomplete mode
        //if((GamePad.GetButtonHold(N3dsButton.L) && GamePad.GetButtonHold(N3dsButton.R)) || (Input.GetKey(KeyCode.T) && Input.GetKey(KeyCode.R))){
        //    if(GameManager.Instance.autocompleteMode)
        //    MoveToChip();
        //}

        if (GameManager.Instance.GetIsPaused())
        {
            return;
        }

        if (GameManager.Instance.GetPlayerInputManager().GetLockOnPressed())
        {
            hasJustPressedLockOn = true;
        }

        //Input booleans. If they are set to 'false' or 'true' they are very likely assigned later in the file via a key/button. 
        bool jumpPressed = false;

        if(!characterModel.activeSelf){
            UnityEngine.Debug.LogWarning("This character model should always be active! Disable the renderer itself instead.");
        }

        if (hasControlOverCharacter)
        {
            lockOn.lockOnButton = hasJustPressedLockOn;

            jumpPressed = GameManager.Instance.GetPlayerInputManager().GetJumpPressed();
            hoverBoardPressed = GameManager.Instance.GetPlayerInputManager().GetHoverboardPressed();

            vertical = Mathf.Lerp(vertical, GameManager.Instance.GetPlayerInputManager().GetLeftStick().y, accelerationLerpRate * Time.deltaTime);
            horizontal = Mathf.Lerp(horizontal, GameManager.Instance.GetPlayerInputManager().GetLeftStick().x, accelerationLerpRate * Time.deltaTime);
        } else 
        {
            vertical = 0f;
            horizontal = 0f;
        }

        float trueVertical = vertical;
        float trueHorizontal = horizontal;

        float deadZone = 0.1f;

        if (hoverBoardPressed) //Trigger once
        {
            hoverBoardForwardMotion = 5f;
            lockOn.lockOnButton = false; //reset lock on, this only gets called one frame
        }

        //joystick deadzone
        if (Mathf.Abs(trueVertical) < deadZone)
        {
            trueVertical = 0f;
        }
        if (Mathf.Abs(trueHorizontal) < deadZone)
        {
            trueHorizontal = 0f;
        }

        if (lockOn && lockOn.isLockedOn && lockOn.target != null && lockOn.target.gameObject.activeSelf)
        {
            characterModel.transform.LookAt(new Vector3(lockOn.target.position.x, characterModel.transform.position.y, lockOn.target.position.z));
        }

        if (lockOn && !lockOn.isLockedOn && playerFire && playerFire.GetArmLifted() && movementType != MovementType.Hoverboard && movementType != MovementType.Teleport)
        {
            characterModel.transform.LookAt(new Vector3(playerFire.bulletTargetPosition.x, characterModel.transform.position.y, playerFire.bulletTargetPosition.z));
        }

        //Gravity
        if (!isGrounded && currentFallSpeed < maxFallSpeed)
        {
            currentFallSpeed += Time.deltaTime * fallAcceleration;
        }

        dashTimer -= Time.deltaTime;
        dashTimer = Mathf.Clamp(dashTimer, 0.05f, 100f);
        dashTrail.enabled = dashTimer > 0.1f;
        dashBackPanel.gameObject.SetActive(dashTrail.enabled);
        if(dashTrail.enabled)
        {
            dashTrail.time = dashTimer * 0.1f + 0.02f;

            dashBackPanel.transform.localScale = initialBackPanelScale * (dashTimer / dashTimerMax);

            //We can assume lensFlare is available at this stage
            lensFlare.transform.localScale = initialLensFlareSize * dashTimer;
        }

        flattenedCamera.transform.rotation = new Quaternion(0f, cameraTransform.rotation.y, 0f, cameraTransform.rotation.w);

        currentSpeed = currentMaxSpeed;

        if (!isGrounded && !justFell && currentFallSpeed >= 0f)
        {
            currentFallSpeed = 0f;
            justFell = true;
        }

        if (isGrounded)
        {
            justFell = false;
        }

        moveDirection.y = Vector3.down.y * currentFallSpeed;

        if (Mathf.Abs(trueVertical) > Mathf.Abs(trueHorizontal))
        {
            speedMultiplier = trueVertical;
        }
        else
        {
            speedMultiplier = trueHorizontal;
        }

        speedMultiplier = Mathf.Abs(speedMultiplier);

        if (animator != null && animator.isActiveAndEnabled)
        {
            animator.SetBool("isGrounded", isGrounded);
            animator.SetFloat("speed", speedMultiplier);
        }

        switch (movementType)
        {
            case MovementType.OnFoot:

                thirdPersonPlayerCamera.cameraMode = ThirdPersonPlayerCamera.CameraMode.standard;

                dashVector = Vector3.Lerp(dashVector, Vector3.zero, 8f * Time.deltaTime);

                if(!playerFire.GetArmLifted())
                    thirdPersonPlayerCamera.rotateAround += horizontal * 90f * Time.deltaTime; //Camera turn, helps keep following player who may be walking around corners

                moveDirection = (flattenedCamera.transform.right * trueHorizontal * currentSpeed) + (flattenedCamera.transform.forward * trueVertical * currentSpeed);
                moveDirection /= 3f;
                moveDirection += (Vector3.down * currentFallSpeed);

                //Dash input
                if (hasControlOverCharacter && GameManager.Instance.GetPlayerInputManager().GetBoostHeld())
                {
                    if (GameManager.Instance.flightMode)
                    {
                        Fly(moveDirection);
                    }

                    if (!GameManager.Instance.flightMode && dashTimer <= dashTime)
                    {
                        Dash(moveDirection);
                    }
                }

                if (!hasJumped && jumpPressed && isGrounded)
                {
                    Jump(jumpStrength);
                }

                if (currentFallSpeed >= 0f && isGrounded)
                {
                    hasJumped = false;
                }

                //Slope sliding (foot)
                if (!justFell && moveDirection.y < 0f && hit.transform && groundShadowRaycastHit.transform && groundShadowRaycastHit.normal.y < 0.7f && groundShadowRaycastHit.normal.y != 0)
                {
                    animator.SetBool("isGrounded", false);
                    slopeVector += ((hit.normal * 50f) + (Vector3.down * 100f)) * Time.deltaTime * 2f;
                    hasJumped = true;
                    isGrounded = false;
                }

                //Ceiling Collision
                if(moveDirection.y > 0f)
                {
                    if(Physics.SphereCast(this.transform.position, characterController.radius, transform.up, out hit, characterController.height / 2f + 0.08f))
                    {
                        //Knock back down
                        Jump(-1f);
                    }
                }

                break;
            case MovementType.Hoverboard:
                thirdPersonPlayerCamera.cameraMode = ThirdPersonPlayerCamera.CameraMode.hoverBoard;

                playerSounds.PlayHoverBoard(hoverBoardForwardMotion / 5f);

                dashVector = Vector3.Lerp(dashVector, Vector3.zero, 16f * Time.deltaTime);

                hoverBoardForwardMotion += vertical * 2f * Time.deltaTime;

                if (vertical < 0.3f && hoverBoardForwardMotion > 0f)
                {
                    hoverBoardForwardMotion -= 8f * Time.deltaTime;
                }

                if (Physics.Raycast(this.transform.position, characterModel.transform.forward, 0.8f))
                {
                    hoverBoardForwardMotion -= 64f * Time.deltaTime;
                }

                characterModel.transform.Rotate(0, horizontal * 90f * Time.deltaTime, 0);

                hoverBoardForwardMotion = Mathf.Clamp(hoverBoardForwardMotion, 0.1f, maxHoverBoardSpeed);

                moveDirection = (characterModel.transform.forward * hoverBoardForwardMotion);

                //Hoverboard fall speed
                currentFallSpeed = 4f;
                moveDirection.y = -2.5f;

                if (!hasJumped && jumpPressed && isGrounded)
                {
                    Jump(jumpStrength);
                }

                animator.SetFloat("speed", 0f);
                animator.SetBool("isGrounded", true);

                //Slope sliding slide (hoverboard)
                if (isGrounded && hit.normal.y < 0.95f)
                {
                    slopeVector += ((hit.normal * 6f)) + (Vector3.down * 3f) * Time.deltaTime * 160f;
                    hasJumped = true;
                }
                break;

            case MovementType.Teleport:
                this.transform.position += 60f * Time.deltaTime * this.transform.forward;

                this.transform.LookAt(destinationTeleportDestination);

                playerSounds.PlayTeleport();

                if (Vector3.Distance(this.transform.position, destinationTeleportDestination.position) < 1f)
                {
                    EndTeleportTravel();
                }
                break;
        }

        if (!lockOn.isLockedOn && hasControlOverCharacter && characterModel != null && Mathf.Abs(trueVertical) + Mathf.Abs(trueHorizontal) > 0.1f && !playerFire.GetArmLifted())
        {
            characterModel.transform.LookAt(transform.position + new Vector3(moveDirection.x * 100f, 0, moveDirection.z * 100f));
        }

        previousYPosition = this.transform.position.y;
    }

    private void FixedUpdate(){
        if (GameManager.Instance.GetIsPaused())
        {
            return;
        }

        if(lensFlare)
        lensFlare.SetActive(dashTimer > 0.08f);

        if(this.transform.position.y < deathYLevel){
            this.GetComponent<PlayerCombat>().Die();
            Die();
        }
    }

    //We need a separate raycast for the slope vector so that steep angles are still caught and calculated
    RaycastHit slopeVectorRaycast;
    private void LateUpdate(){
        if (GameManager.Instance.GetIsPaused())
        {
            return;
        }

        bool hasGroundShadow = Physics.Raycast(this.transform.position, -transform.up, out groundShadowRaycastHit, characterController.height / 2f + 40f) && groundShadowRaycastHit.transform.gameObject != this.gameObject;
        shadow.SetActive(hasGroundShadow && movementType != MovementType.Hoverboard);
		trueShadow.gameObject.SetActive(hasGroundShadow && movementType != MovementType.Hoverboard);
        isGrounded = Physics.SphereCast(this.transform.position, characterController.radius, -transform.up, out hit, characterController.height / 2f + 0.025f) && currentFallSpeed > 0f;

        //Physics.Raycast(this.transform.position, -transform.up, out slopeVectorRaycast, characterController.height / 2f + 40f);

        // UnityEngine.Debug.DrawRay (rch.point, rch.normal, Color.red, 0.2f);
        shadow.transform.LookAt (groundShadowRaycastHit.point + groundShadowRaycastHit.normal + (characterModel.transform.forward/256f));

        //Reset to be always under player, if this line of code is not in place the physics updates slower than Update() (It uses fixedUpdate) so the shadow will lag behind as player moves fast.
		shadow.transform.position = new Vector3(this.transform.position.x, groundShadowRaycastHit.point.y + (isGrounded ? 0.01f : 0.04f), this.transform.position.z);

        float isMovingMultiplier = Mathf.Abs(moveDirection.x + moveDirection.z) > 0.05f ? 1f : 0.001f;

		//Set shadow rotation properly
        if(groundShadowRaycastHit.normal.y < 0.999f)
        {
            trueShadow.transform.position = shadow.transform.position + ((groundShadowRaycastHit.normal/3f) * isMovingMultiplier * 0.2f);
        } else
        {
            trueShadow.transform.position = shadow.transform.position;
        }
        trueShadow.transform.rotation = shadow.transform.rotation;
        trueShadow.transform.Rotate (0f, 0f, -(trueShadow.eulerAngles.y - this.transform.eulerAngles.y));

        //Moving platforms / elevators
        //Is there a moving platform under our feet?
        if(groundShadowRaycastHit.transform && groundShadowRaycastHit.transform.gameObject.GetComponent<Lift>() && ((Vector3.Distance(this.transform.position, groundShadowRaycastHit.transform.position) < 5f) || groundShadowRaycastHit.transform.GetComponent<Lift>().ignoreDistance)){
            this.transform.parent = groundShadowRaycastHit.transform;
            shadow.SetActive(false);
            trueShadow.gameObject.SetActive(false);
        } else {
            this.transform.parent = null;
        }

        slopeVector = Vector3.Lerp(slopeVector, Vector3.zero, 6f * Time.deltaTime);

        if (lockOn.isLockedOn && lockOn.target && Vector3.Distance(new Vector3(this.transform.position.x, 0f, this.transform.position.z), new Vector3(lockOn.target.transform.position.x, 0f, lockOn.target.transform.position.z)) < 1f)
        {
            moveDirection -= characterModel.transform.forward * moveDirection.magnitude;
        }

        if(!hasControlOverCharacter)
        {
            moveDirection = new Vector3(0f, moveDirection.y, 0f);
            animator.SetFloat("speed", 0f);
        }

        if (this.characterController.enabled)
        characterController.Move((moveDirection + dashVector + slopeVector) * Time.deltaTime);
        
        //Used for removing the y aspect of shadow which is parented to this transform.
        if(this.transform.position.y != previousYPosition){
            float difference = previousYPosition - this.transform.position.y;
            trueShadow.transform.position += Vector3.up * difference;
            shadow.transform.position += Vector3.up * difference;
        }

        shadow.transform.rotation = characterModel.transform.rotation;
        shadow.transform.Rotate(-90f, 0f, 0f);
    }

    private void Dash(Vector3 currentMoveDirection)
    {
        if(dead || GameManager.Instance.GetIsPaused()){
            return;
        }

        if(playerSounds)
        playerSounds.PlayBoost();

        float multiplier = isGrounded ? 2f : 1f;

        currentFallSpeed = -1f; //Reset fall speed in dash
        hasJumped = true;

        this.dashVector = new Vector3(currentMoveDirection.x, !isGrounded ? 12f : 0.6f, currentMoveDirection.z) * dashStrength * multiplier;
        dashTimer = dashTimerMax;

        isGrounded = false;
    }

    private void Fly(Vector3 currentMoveDirection){
        if(dashTrail)
            dashTrail.time = 0.08f;
        
        this.currentFallSpeed = 0f;
        this.dashVector = new Vector3(currentMoveDirection.x, 1.5f, currentMoveDirection.z) * dashStrength * Time.deltaTime * 120f;
    }

    public void Jump(float strength)
    {
        currentFallSpeed = -strength;
        hasJumped = true;
    }

    public void BeginTeleportTravel(Transform teleportDestination)
    {
        hasControlOverCharacter = false;

        teleportParticles.Play();

        destinationTeleportDestination = teleportDestination;

        previousMovementType = this.movementType;

        this.movementType = MovementType.Teleport;
        this.characterController.enabled = false;

        foreach(Renderer r in characterRenderers)
        {
            r.enabled = false;
        }

        this.transform.LookAt(teleportDestination.position);

        justTeleported = true;
    }

    public void EndTeleportTravel(){
        this.dashVector = Vector3.zero;
        this.moveDirection = Vector3.zero;

        this.transform.position = destinationTeleportDestination.position + (Vector3.up/4f);

        if(teleportParticles)
            teleportParticles.Stop();

        this.characterController.enabled = true;

        foreach (Renderer r in characterRenderers)
        {
            r.enabled = true;
        }

        this.transform.rotation = Quaternion.identity;

        this.movementType = previousMovementType;

        hoverboard.ToggleBeingUsed();
        hoverboard.ToggleBeingUsed();

        Jump(6f);
        hasControlOverCharacter = true;

        if(shadow && trueShadow){
        shadow.gameObject.SetActive(true);
        trueShadow.gameObject.SetActive(true);
        }

        Invoke("AllowNextTeleport", 0.6f);
    }

    private void AllowNextTeleport(){
        justTeleported = false;
    }

    public void SetMovementType(MovementType newMovementType)
    {
        this.movementType = newMovementType;
    }

    public MovementType GetMovementType()
    {
        return this.movementType;
    }

/// <summary>
/// Disables all input and movement aside from gravity accross hoverboard and foot movement.
/// </summary>
    public void Die(){
        dead = true;

        currentSpeed = 0;
        currentMaxSpeed = 0;
        maxHoverBoardSpeed = 0;

        hasJumped = true;

        movementType = MovementType.OnFoot;

        if(lockOn){
        lockOn.isLockedOn = false;
        lockOn.target = null;
        }

        hasControlOverCharacter = false;
        moveDirection = new Vector3(0f, -32f, 0f);

        dashVector = Vector3.zero;

        animator.SetBool("dead", true);
    }

    private void MoveToChip(){
        Collectible chip = GameManager.Instance.allCollectiblesInCurrentScene[Random.Range(0,GameManager.Instance.allCollectiblesInCurrentScene.Count)];

        if(chip && chip.GetComponent<ChipCollectible>() && !chip.hasBeenCollected){
            this.characterController.enabled = false;
            this.transform.position = chip.transform.position;
            this.characterController.enabled = true;
        } else {
            Lift l = FindObjectOfType<Lift>();

            if(l && l.gameObject.tag == "IgnoreBullets"){
                l.gameObject.SetActive(false);
            }
        }
    }
}
