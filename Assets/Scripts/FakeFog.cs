using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeFog : MonoBehaviour {

	private Transform follow;

	public float fogStartDistance = 30f;
	public float fogEndDistance = 600f;

	private BeetleFlight bf;

	private void Start(){
		Destroy(this.gameObject);

		if(GameManager.Instance && GameManager.Instance.thirdPersonCharacterMovement)
			bf = GameManager.Instance.thirdPersonCharacterMovement.GetComponent<BeetleFlight>();

		GameManager.Instance.fakeFog = this.gameObject;

		fogStartDistance -= 24f;
		fogEndDistance -= 200f;

		fogStartDistance = Mathf.Clamp(fogStartDistance, 10f, fogEndDistance);
	 	fogEndDistance = Mathf.Clamp(fogEndDistance, fogStartDistance, Mathf.Infinity);

		follow = Camera.main.transform;

		this.transform.position = follow.transform.position;
		this.transform.Rotate(180f, 180f, 0f);

		this.transform.localScale = new Vector3(this.transform.localScale.x, this.transform.localPosition.y * fogEndDistance / 100f, this.transform.localScale.z);

		this.transform.parent = follow.transform;

		this.transform.position += this.transform.up * fogStartDistance;

		if(bf){
			this.transform.localScale = new Vector3(this.transform.localScale.x * 20f, this.transform.localScale.y, this.transform.localScale.z * 20f);
		}
	}
}
