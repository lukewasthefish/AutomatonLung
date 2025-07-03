using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScootingEnemy : Enemy
{
    public float turnRate = 45f;

    private bool canFire = false;

    protected override void Awake()
    {
        InvokeRepeating("FireTrigger", 0f, 0.3f);
    }

    public ScootingEnemy(CharacterController cc, MeshRenderer mr, MeshFlash mf) : base(cc, mf)
    {
    }

    protected override void Update()
    {
        base.Update();

        baseMoveDirection = this.transform.forward * moveSpeed;

        if(!IsWithinSight(GameManager.Instance.thirdPersonCharacterMovement.transform, 5f))
        {
            canFire = false;
            this.transform.Rotate(0f, turnRate * Time.deltaTime, 0f);
        } else
        {
            canFire = true;
        }
    }

    private void FireTrigger()
    {
        if (canFire)
        {
            Fire(GameManager.Instance.thirdPersonCharacterMovement.transform.position + (Vector3.up / 4f), 8f);
        }
    }

    protected override void Fire(Vector3 targetPosition, float bulletMovementSpeed)
    {
        base.Fire(targetPosition, bulletMovementSpeed);
    }
}
