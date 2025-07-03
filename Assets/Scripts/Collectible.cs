using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Adds a key to save data when the player has collided with its required SphereCollider component.
/// This is designed to be extended from, and should not be added as is.
/// For example, if you're trying to create a health pickup you should not add this component to your health pickup.
/// Instead extend from this with your own custom health pickup class.
/// </summary>
[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(CullThisObject))]
public abstract class Collectible : MonoBehaviour {
    protected string key;

    private SphereCollider sphereCollider;
    private CullThisObject cullThisObject;

    [HideInInspector]public bool hasBeenCollected = false;

    protected virtual void Awake()
    {
        sphereCollider = GetComponent<SphereCollider>();
        cullThisObject = GetComponent<CullThisObject>();
        sphereCollider.enabled = false;
        sphereCollider.isTrigger = true;

        Invoke("ActivateSphereCollider", 0.2f);
    }

    /// <summary>
    /// Delay for sphere collider activation so that if player isn't aligned with their playerstart and they happen to collide with this it isn't collected.
    /// </summary>
    private void ActivateSphereCollider(){
        sphereCollider.enabled = true;
    }

    private void Start()
    {
        GameManager.Instance.allCollectiblesInCurrentScene.Add(this);

        //Rounding to prevent giant floating numbers in the save data.
        //Saving super huge numbers in the data can present crashes on the target platform.
        int xPos = Mathf.RoundToInt(this.transform.position.x);
        int yPos = Mathf.RoundToInt(this.transform.position.y);
        int zPos = Mathf.RoundToInt(this.transform.position.z);

        key = xPos + "" + yPos + "" + zPos + "" + SceneManager.GetActiveScene().name;

        //Has this been collected previously?
        if (GameManager.Instance.CurrentSaveData != null && GameManager.Instance.CurrentSaveData.ContainsKey(key) && GameManager.Instance.CurrentSaveData.GetInt(key) == 1)
        {
            cullThisObject.cullingEnabled = false;
            hasBeenCollected = true;
            this.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        this.transform.Rotate(0f, 180f * Time.deltaTime, 0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        ThirdPersonCharacterMovement thirdPersonCharacterMovement = other.GetComponent<ThirdPersonCharacterMovement>();

        if (thirdPersonCharacterMovement != null)
        {
            cullThisObject.enabled = false;

            CollectibleAction(thirdPersonCharacterMovement);

            //this.transform.position = Vector3.one * 999f; //For culling scripts. let's just get this object out of the way.

            GameManager.Instance.CurrentSaveData.SetValue(key, 1);

            hasBeenCollected = true;

            //cullThisObject.cullingEnabled = false;
            Destroy(this.gameObject);
        }
    }

    /// <summary>
    /// To be overriden.
    /// Will run whatever necessary code for that specific type of collectible.
    /// For example, a health pickup will increase the maximum player health in this function.
    /// </summary>
    protected abstract void CollectibleAction(ThirdPersonCharacterMovement thirdPersonCharacterMovement);
}
