using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Player movement when flying through the overworld.
/// </summary>
public class BeetleFlight : MonoBehaviour {

    public float topSpeed = 5f;
    public float rotateSpeed = 5f;

    private float currentRotateSpeed;
    private float destinationRotateSpeed;

    public TrailRenderer[] trails;

    public float CurrentMoveSpeed { get; private set; }

    private Transform cameraTransform;

    private bool canReceiveInput = true;

    private BeetleFlightSounds beetleFlightSounds;

    private void Awake()
    {
        cameraTransform = Camera.main.transform;

        beetleFlightSounds = GetComponent<BeetleFlightSounds>();
    }

    private void Start()
    {
        this.transform.rotation = Quaternion.identity;

        GameManager.Instance.beetleFlight = this;
    }

    private const float moveSpeedTurnAdditionDivisor = 8f;
    private void Update()
    {
        //Turning
        destinationRotateSpeed = Mathf.Clamp(rotateSpeed - CurrentMoveSpeed, 0f, rotateSpeed);
        currentRotateSpeed = Mathf.Lerp(currentRotateSpeed, destinationRotateSpeed, 1f * Time.deltaTime);

        Vector2 turnRate = new Vector2(GameManager.Instance.GetPlayerInputManager().GetLeftStick().y * (currentRotateSpeed + (CurrentMoveSpeed / moveSpeedTurnAdditionDivisor)), GameManager.Instance.GetPlayerInputManager().GetLeftStick().x * (currentRotateSpeed + (CurrentMoveSpeed / moveSpeedTurnAdditionDivisor)));

        if (this.transform.eulerAngles.x > 300f || this.transform.eulerAngles.x < 60f) //Clamp pitch to prevent strange loop-de-loops
        {
            this.transform.Rotate(turnRate * Time.deltaTime);
        }

        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, new Quaternion(0f, cameraTransform.rotation.y, 0f, cameraTransform.rotation.w), 1f * Time.deltaTime);

        if (!canReceiveInput) return;

        float destinationSpeed = GameManager.Instance.GetPlayerInputManager().GetJumpHeld() ? topSpeed : 0f;

        CurrentMoveSpeed = Mathf.Lerp(CurrentMoveSpeed, destinationSpeed, 0.4f * Time.deltaTime);

        Vector3 forwardMotion = this.transform.forward * CurrentMoveSpeed * 1.5f;
        this.transform.position += forwardMotion * Time.deltaTime;

        beetleFlightSounds.FlightEngine(CurrentMoveSpeed / 50f);

        //Halt progress when reaching a wall
        RaycastHit hit;
        if(Physics.SphereCast(this.transform.position, 2f, this.transform.forward, out hit, 20f))
        {
            this.transform.position -= forwardMotion * Time.deltaTime;
            this.CurrentMoveSpeed = 0f;
        }
    }

    private void OnEnable()
    {
        if(trails.Length > 0)
        {
            for(int i = 0; i < trails.Length; i++){
                trails[i].enabled = true;
            }
        }
    }

    private void OnDisable()
    {
        if (trails.Length > 0)
        {
            for(int i = 0; i < trails.Length; i++){
                trails[i].enabled = false;
            }
        }
    }

    /// <summary>
    /// Clear all trails. Useful for when sharply and suddenly moving this GameObjects Transform. Prevents trails from creating long unwanted lines accross the sky.
    /// </summary>
    public void ResetTrails()
    {
        if (trails.Length > 0)
        {
            for(int i = 0; i < trails.Length; i++){
                trails[i].Clear();
            }
        }
    }

    /// <summary>
    /// Completely halt all flight motion.
    /// </summary>
    public void StopAllMovement()
    {
        this.CurrentMoveSpeed = 0;
        canReceiveInput = false;
    }
}
