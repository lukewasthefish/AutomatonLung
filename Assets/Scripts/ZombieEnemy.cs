using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieEnemy : Enemy {

    public ZombieEnemy(CharacterController cc, MeshFlash mf) : base(cc, mf)
    {
    }

	protected override void Awake(){
        base.Awake();
		InvokeRepeating("Look", 0f, 2f);
	}

    private void OnEnable()
    {
        this.knockback = Vector3.zero;
    }

    protected override void Update()
    {
        base.Update();
		this.transform.position += this.transform.forward * moveSpeed * Time.deltaTime;
    }

	private void Look(){
        if (!GameManager.Instance.thirdPersonCharacterMovement.GetComponent<PlayerCombat>().dead)
        {
            this.transform.LookAt(GameManager.Instance.thirdPersonCharacterMovement.transform.position);
        } else
        {
            this.transform.LookAt(Vector3.one * 1000f);
        }
	}
}
