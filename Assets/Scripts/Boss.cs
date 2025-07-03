using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public abstract class Boss : Enemy
{
    [Header("Location of the bosses weakpoint for the player to look at when attacking.")]
    public Transform lockOnLocation;

    public bool loadSceneAfterDefeat = true;
    public string sceneToLoad = "";

    [Tooltip("List of all colliders that the player can attack to cause damage to this boss.\nCan be left empty if desired, character controller will always take damage.")]
    public EnemyHurtBox[] hurtBoxes;

    [Header("Optional")]
	public UnityEvent eventToCall;

    protected override void Awake(){
        base.Awake();
    }

    protected override void Update(){
        base.Update();

        if (!this.defeated && this.currentHealth <= 0)
        {
            this.BossDeath();
            this.defeated = true;
            return;
        }
    }

    public Boss(CharacterController cc, MeshFlash mf) : base(cc, mf)
    {
        this.isBoss = true;

        cc = GetComponent<CharacterController>();
        mf = GetComponent<MeshFlash>();
    }

    /// <summary>
    /// To be overriden. Varies greatly dependant upon the Boss it is being called on.
    /// </summary>
    private void BossDeath()
    {
        eventToCall.Invoke();

        BossDeathAction();

        this.transform.position = Vector3.one * 9999f;
        this.gameObject.SetActive(false);
    }

    protected virtual void BossDeathAction(){

    }

    protected void LoadSceneAfterDefeat()
    {
        GameManager.Instance.LoadAfterDelay(1f, sceneToLoad);
    }
}