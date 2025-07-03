using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeballEnemy : Enemy
{
	public Explosion explosion;

	private int initialHealth;

	private MeshRenderer meshRenderer;

	private bool explosionTriggered = false;

	private bool createdNewExplosion = false;

	protected override void Awake(){

		meshRenderer = GetComponent<MeshRenderer>();

		initialHealth = maxHealth;
	}

    public SpikeballEnemy(CharacterController cc, MeshFlash mf) : base(cc, mf) {}

	private void OnEnable()
	{
		meshRenderer.enabled = true;
		explosionTriggered = false;
	}

    protected override void Update()
    {
		base.Update();

        this.transform.Rotate(64f * Time.deltaTime, 15f * Time.deltaTime, 45f * Time.deltaTime);

		//If attacked or touched by player; do exploding sequence
		if((this.currentHealth < initialHealth || Vector3.Distance(GameManager.Instance.thirdPersonCharacterMovement.transform.position, this.transform.position) < 2f) && !explosionTriggered)
		{
			if (!createdNewExplosion)
			{
				explosion = Instantiate(explosion, null);
				createdNewExplosion = true;
			}

			explosion.gameObject.SetActive(true);

			this.enemySpawnPoint.hasBeenTriggered = true;

			explosion.transform.position = this.transform.position;

			explosion.Explode();
			explosion.transform.parent = null;

			explosionTriggered = true;

			meshRenderer.enabled = false;

			this.currentHealth = 0;
		}
    }
}
