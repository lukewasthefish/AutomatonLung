using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class Hoverboard : MonoBehaviour {

    public TrailRenderer trailRenderer;

    public GameObject lensFlare;

    public ThirdPersonCharacterMovement thirdPersonCharacterMovement;

    [Header("Bone to use for this objects tranform parent when it is being ridden.")]
    public Transform parentBone; //For animation purposes

    private bool beingUsed = false;

    private MeshRenderer meshRenderer;

    private Vector3 initialScale;
    private Vector3 lensFlareInitialScale;

    private void Awake()
    {
        initialScale = this.transform.lossyScale;
        lensFlareInitialScale = lensFlare.transform.localScale;

        meshRenderer = GetComponent<MeshRenderer>();

        trailRenderer.enabled = false;
        meshRenderer.enabled = false;
    }

    private void Update()
    {
        if (GameManager.Instance.GetIsPaused())
        {
            return;
        }

        if (!thirdPersonCharacterMovement)
        {
            return;
        }

        if (thirdPersonCharacterMovement.hoverBoardPressed && (thirdPersonCharacterMovement.GetMovementType() == ThirdPersonCharacterMovement.MovementType.Hoverboard || thirdPersonCharacterMovement.isGrounded))
        {
            ToggleBeingUsed();
        }

        if (beingUsed)
        {
            lensFlare.SetActive(true);
            this.transform.localScale = Vector3.Lerp(this.transform.localScale, initialScale * 100f, 2f * Time.deltaTime);
            lensFlare.transform.localScale = (lensFlareInitialScale/3f) + (lensFlareInitialScale * thirdPersonCharacterMovement.hoverBoardForwardMotion / 25f);
        }
        else
        {
            lensFlare.SetActive(false);
            this.transform.localScale = Vector3.Lerp(this.transform.localScale, 0.04f * Vector3.one, 2f * Time.deltaTime);
        }
    }

    public void DisableVisuals()
    {
        trailRenderer.enabled = false;
    }

    public void EnableVisuals()
    {
        trailRenderer.enabled = true;
    }

    public void ToggleBeingUsed()
    {
        beingUsed = !beingUsed;
        trailRenderer.enabled = !trailRenderer.enabled;

        if (beingUsed)
        {
            this.transform.position = parentBone.position;
            this.transform.rotation = parentBone.rotation;

            //Manually rotate the hoverboard so that it faces the correct direction
            this.transform.Rotate(-90, 0, 0);
            this.transform.Rotate(0, 180, 0);

            meshRenderer.enabled = true;
            this.transform.parent = parentBone;

            thirdPersonCharacterMovement.SetMovementType(ThirdPersonCharacterMovement.MovementType.Hoverboard);
        } else
        {
            meshRenderer.enabled = false;
            this.transform.parent = null;

            thirdPersonCharacterMovement.SetMovementType(ThirdPersonCharacterMovement.MovementType.OnFoot);
        }
    }

    public bool IsBeingUsed()
    {
        return beingUsed;
    }
}
