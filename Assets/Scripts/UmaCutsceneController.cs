using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UmaCutsceneController : MonoBehaviour {

    public bool walking = false;

    public float speed = 0f;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        animator.SetFloat("speed", speed);

        transform.position += this.transform.forward * speed * 9f * Time.deltaTime;
    }
}
