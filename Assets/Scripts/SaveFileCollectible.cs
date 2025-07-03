using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(MeshFlash))]
[RequireComponent(typeof(SphereCollider))]
/// <summary>
/// Checkpoint. This class does not extend collectible.
/// </summary>
public class SaveFileCollectible : MonoBehaviour {

    private MeshFlash meshFlash;

    private void Awake()
    {
        meshFlash = GetComponent<MeshFlash>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<ThirdPersonCharacterMovement>() != null && Time.timeSinceLevelLoad > 1f)
        {
            meshFlash.Flash(1f);

            GameManager.Instance.SetDestinationPlayerStart(-1); //-1 refers to the playerStart attached to the saveFile prefab

            GameManager.Instance.CurrentSaveData.SetValue("saveScene", SceneManager.GetActiveScene().name);

            GameManager.Instance.Save();
        }
    }

    private void Update()
    {
        this.transform.Rotate(0, Mathf.Sin(Time.timeSinceLevelLoad) * Mathf.Cos(Time.timeSinceLevelLoad/2f) * 250f * Time.deltaTime, 0, 0);
    }
}
