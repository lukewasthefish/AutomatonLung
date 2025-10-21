using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Special collision for the overworld flight areas that allows different collision modes when the beetle ship has crashed, etc.
/// </summary>
[RequireComponent(typeof(BeetleFlight))]
[RequireComponent(typeof(Rigidbody))]
public class BeetleCollision : MonoBehaviour {

    public float beetleRadius = 2.4f;

    private Rigidbody thisRigidbody;
    private BeetleFlight beetleFlight;

    //public ParticleSystem crashSmoke;
    public ParticleSystem explosion;

    private bool isTumbling = false; //Is the beetle falling after a crash?
    [HideInInspector] public bool crashed = false;

    private void Awake()
    {
        beetleFlight = GetComponent<BeetleFlight>();
        thisRigidbody = GetComponent<Rigidbody>();

        thisRigidbody.useGravity = false;
        thisRigidbody.isKinematic = false;
    }

    private void Update()
    {
        beetleFlight.enabled = !isTumbling;

        //Crashing at higher speeds
        if(!isTumbling && beetleFlight.CurrentMoveSpeed > beetleFlight.topSpeed / 1.4f && Physics.CheckSphere(this.transform.position, beetleRadius, LayerMask.GetMask("Default")))
        {
            Debug.Log("CRASHED BEETLE");

            isTumbling = true;

            thisRigidbody.useGravity = true;
            thisRigidbody.linearDamping = 0f;

            thisRigidbody.AddForce(-transform.forward * 5000f + (Vector3.up * 50f), ForceMode.Force);
            //thisRigidbody.AddTorque(Vector3.one * 50f, ForceMode.Impulse);

            // CrashBeetle();
        }

        /*
        if(!smokeEnabled && crashed && thisRigidbody.velocity.magnitude < 0.2f)
        {
            ParticleSystem newSmoke = Instantiate(crashSmoke, null);
            newSmoke.transform.position = this.transform.position;

            smokeEnabled = true;
        }
        */
    }

    private void CrashBeetle()
    {
        // crashed = true;

        // ParticleSystem newExplosion = Instantiate(explosion, null);
        // newExplosion.transform.position = this.transform.position;

        // Camera.main.GetComponent<BeetleFlightCamera>().crashCam = true;

        // Invoke("Reload", 11f);
    }

    private void ResetTumbling()
    {
        isTumbling = false;
    }
}
