using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DeathTrigger : MonoBehaviour {

    private Collider thisCollider;

    private void Awake()
    {
        thisCollider = GetComponent<Collider>();
        thisCollider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerCombat pc = other.GetComponent<PlayerCombat>();

        if(pc != null)
        {
            pc.Die();
        }
    }
}
