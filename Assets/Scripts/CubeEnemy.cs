using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeEnemy : Enemy
{
    public CubeEnemy(CharacterController cc, MeshFlash mf) : base(cc, mf)
    {
    }

    protected override void Awake()
    {
		CancelInvoke ("CallFire");
        InvokeRepeating("CallFire", 1f, 2f);
    }

    private void CallFire()
    {
        if(GameManager.Instance && GameManager.Instance.thirdPersonCharacterMovement)
        this.Fire(GameManager.Instance.thirdPersonCharacterMovement.transform.position, 18f);
    }

    protected override void Update()
    {
        base.Update();

        this.transform.Rotate(59f * Time.deltaTime, 0f, 0f);
    }
}
