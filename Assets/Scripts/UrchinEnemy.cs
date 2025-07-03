using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UrchinEnemy : Enemy {

    public UrchinEnemy(CharacterController cc, MeshFlash mf) : base(cc, mf)
    {
    }

    private void OnEnable()
    {
        this.knockback = Vector3.zero;
    }

    protected override void Update()
    {
        base.Update();

		this.transform.LookAt(GameManager.Instance.thirdPersonCharacterMovement.transform);
		this.knockback += this.transform.forward * moveSpeed * Time.deltaTime;
		this.characterModel.transform.Rotate(200f * Time.deltaTime, 10f * Time.deltaTime, 12f * Time.deltaTime);
    }
}
