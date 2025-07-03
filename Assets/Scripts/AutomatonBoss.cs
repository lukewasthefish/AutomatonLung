using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomatonBoss : Boss {

    private float initialMoveSpeed;

    private Animator thisAnimator;

    public EnemySpawnPoint newEnemySpawnPoint;

    public AutomatonBoss(CharacterController cc, MeshFlash mf) : base(cc, mf)
    {
    }

    protected override void Awake()
    {
        base.Awake();

        thisAnimator = GetComponent<Animator>();
        initialMoveSpeed = this.moveSpeed;

        InvokeRepeating("TryToSpawnAnEnemy", 5f, 8f);
    }

    protected override void Update()
    {
        base.Update();
        this.knockback = Vector3.zero;

        moveSpeed = ((maxHealth - currentHealth)/100f) + initialMoveSpeed;

        this.characterController.Move(transform.forward * moveSpeed * Time.deltaTime);

        //this.transform.position += transform.forward * 22f * Time.deltaTime;
        this.transform.Rotate(0f, 8f * (moveSpeed / initialMoveSpeed) * Time.deltaTime, 0f);

        thisAnimator.SetFloat("speed", moveSpeed / initialMoveSpeed);
    }

    protected override void BossDeathAction()
    {
        if (SteamManager.Initialized && GameManager.Instance.canAchieveAchievements)
        {
            Steamworks.SteamUserStats.SetAchievement("Achievement_AutomatonLung");
            Steamworks.SteamUserStats.StoreStats();
        }

        LoadSceneAfterDefeat();
    }

    private void TryToSpawnAnEnemy(){
        newEnemySpawnPoint.hasBeenTriggered = false;
    }
}
