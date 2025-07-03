using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingEnemy : Enemy {

    public JumpingEnemy(CharacterController cc, MeshFlash mf) : base(cc, mf)
    {
    }
    protected override void Awake()
    {
        base.Awake();

		CancelInvoke ("Jump");
        InvokeRepeating("Jump", Random.Range(0.1f, 3f), 2f);
    }

    private void OnEnable()
    {
        this.knockback = Vector3.zero;
    }

    private void Jump()
    {
        if(GameManager.Instance.thirdPersonCharacterMovement)
            this.Fire(GameManager.Instance.thirdPersonCharacterMovement.transform.position, 15f);

        this.currentFallSpeed = -10f;

        int roll = Mathf.RoundToInt(Random.Range(0, 1f));
        if(roll == 1)
        {
            this.knockback += this.transform.right * moveSpeed;
        } else
        {
            this.knockback -= this.transform.right * moveSpeed;
        }

        this.knockback += this.transform.forward * moveSpeed * 2f;
    }

    protected override void Update()
    {
        base.Update();

        if(GameManager.Instance.thirdPersonCharacterMovement)
            this.transform.LookAt(GameManager.Instance.thirdPersonCharacterMovement.transform.position);

        if(currentFallSpeed < 0f)
        {
            this.characterModel.transform.Rotate(0f, 700f * Time.deltaTime, 0f);
        } else
        {
            this.characterModel.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.identity, 2f * Time.deltaTime);
        }
    }
}
