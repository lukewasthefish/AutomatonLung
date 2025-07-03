using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class ReflectionMirrorAnimations : MonoBehaviour {

    public Animator animatorToMirror;

    private Animator thisAnimator;

    private void Awake()
    {
        thisAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        for(int i = 0; i < animatorToMirror.parameters.Length; i++)
        {
            thisAnimator.parameters[i] = animatorToMirror.parameters[i];
        }
    }
}
