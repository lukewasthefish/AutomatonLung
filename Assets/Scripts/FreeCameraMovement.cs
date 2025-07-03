using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeCameraMovement : MonoBehaviour
{
    private float currentMoveSpeed = 0f;

    private float baseMoveSpeed = 1f;
    private float addAmmount = 20f;
    private float maxSpeed = 100f;

    private float rotationMultiplier = 4f;

    private ThirdPersonPlayerCamera thirdPersonPlayerCamera;

    private bool useThis = false;

    private	Vector2 rotation = Vector2.zero;

    private float rotateVectorLimit = 21;

    private void Awake()
    {
        currentMoveSpeed = baseMoveSpeed;
        thirdPersonPlayerCamera = GetComponent<ThirdPersonPlayerCamera>();
    }

    private void LateUpdate()
    {
        if(!useThis)
        {
            return;
        }

        thirdPersonPlayerCamera.enabled = false;

        float thrust = GameManager.Instance.GetPlayerInputManager().GetLeftStick().y;
        float strafe = GameManager.Instance.GetPlayerInputManager().GetLeftStick().x;

        float yaw = GameManager.Instance.GetPlayerInputManager().GetRightStick().x;
        float pitch = GameManager.Instance.GetPlayerInputManager().GetRightStick().y;

        //Rotate
		rotation.y += yaw;
		rotation.x += -pitch;
        if(rotation.x < -rotateVectorLimit)
        {
            rotation.x = -rotateVectorLimit;
        }

        if(rotation.x > rotateVectorLimit)
        {
            rotation.x = rotateVectorLimit;
        }
		this.transform.eulerAngles = (Vector2)rotation * rotationMultiplier;

        //Translate
        if(Mathf.Abs(thrust) + Mathf.Abs(strafe) > 0f)
        {
            currentMoveSpeed += addAmmount * Time.deltaTime;
        } else 
        {
            currentMoveSpeed = baseMoveSpeed;
        }

        if(currentMoveSpeed > maxSpeed)
        {
            currentMoveSpeed = maxSpeed;
        }

        this.transform.position += ( (thrust * this.transform.forward) + (strafe * this.transform.right) ) * currentMoveSpeed * Time.deltaTime;
    }

    public void EnableFreecam()
    {
        UnityEngine.Debug.Log("Enabling freecam");
        useThis = true;

        ThirdPersonCharacterMovement tpcm = FindObjectOfType<ThirdPersonCharacterMovement>(true);
        tpcm.DisableControlOverCharacter();
    }

    public void DisableFreecam()
    {
        if(useThis == true)
        {
            UnityEngine.Debug.Log("Disabling freecam");
            useThis = false;
            thirdPersonPlayerCamera.enabled = true;
            ThirdPersonCharacterMovement tpcm = FindObjectOfType<ThirdPersonCharacterMovement>(true);
            tpcm.EnableControlOverCharacter();
        }
    }
}
