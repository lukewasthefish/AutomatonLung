using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFOEnemy : Enemy {

    private float initialMoveSpeed;

    public UFOEnemy(CharacterController cc, MeshFlash mf) : base(cc, mf)
    {
    }
    protected override void Awake()
    {
        base.Awake();

        initialMoveSpeed = this.moveSpeed;

        CancelInvoke();

        InvokeRepeating("PickNewDirection", Random.Range(0.1f, 3f), 2f);
		InvokeRepeating("InitiateFire", 2f, 1f);
    }

    private void OnEnable()
    {
        this.knockback = Vector3.zero;
    }

	private void PickNewDirection(){
		this.transform.Rotate(0f, 0f, Random.Range(0f, 360f));
        moveSpeed = Random.Range(0f, initialMoveSpeed * 1.4f);
	}

	private void InitiateFire(){
		Fire(-GameManager.Instance.thirdPersonCharacterMovement.transform.position, 9f);
	}

    protected override void Update()
    {
        base.Update();

		knockback = this.transform.right * Time.deltaTime * (moveSpeed/2f);
    }
}
