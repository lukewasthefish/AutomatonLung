using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStart : MonoBehaviour {

    public int startIndex = 0;

    private Transform thisPlayerTransform;

    private void Start()
    {
        if (GameManager.Instance && GameManager.Instance.thirdPersonCharacterMovement && GameManager.Instance.thirdPersonCharacterMovement.transform)
        {
            thisPlayerTransform = GameManager.Instance.thirdPersonCharacterMovement.transform;
        }

        if (!thisPlayerTransform)
        {
            ThirdPersonCharacterMovement thirdPersonCharacterMovement = FindObjectOfType<ThirdPersonCharacterMovement>();

            if (thirdPersonCharacterMovement)
            {
                thisPlayerTransform = thirdPersonCharacterMovement.transform;
            }
        }

        if (!thisPlayerTransform)
        {
            BeetleFlight beetleFlight = FindObjectOfType<BeetleFlight>();

            if (beetleFlight)
            {
                thisPlayerTransform = beetleFlight.transform;
            }
        }

        AssignPlayerToPosition();
        Destroy(this.gameObject);
    }

    private void AssignPlayerToPosition()
    {
        if (!GameManager.Instance)
        {
            Debug.LogWarning("GameManager was not assigned");
            return;
        }

        if (!thisPlayerTransform)
        {
            Debug.LogWarning("Player transform was not assigned");
            return;
        }

        if (this.startIndex == GameManager.Instance.GetDestinationPlayerStart())
        {
            thisPlayerTransform.SetPositionAndRotation(this.transform.position + Vector3.up, this.transform.rotation);
            thisPlayerTransform.transform.Rotate(0f, -35f, 0f);
        }
    }
}
