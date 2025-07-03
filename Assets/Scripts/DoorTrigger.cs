using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorTrigger : MonoBehaviour {

    public string destinationScene = "Title Menu";

    [Header("Which Player Start to send the player to in the destinationScene.")]
    public int destinationPlayerStart = 0;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            Camera[] allCameras = FindObjectsOfType<Camera>(true);

            foreach (Camera cam in allCameras)
            {
                //Blind cameras
                cam.nearClipPlane = 0.5f;
                cam.farClipPlane = 1f;
                cam.transform.parent = null;
            }

            if (SteamManager.Initialized && SceneManager.GetActiveScene().name.Contains("TownD") && destinationScene == "Floor 0")
            {
                Steamworks.SteamUserStats.SetAchievement("Achievement_FallFromTower");
                Steamworks.SteamUserStats.StoreStats();
            }

            ThirdPersonCharacterMovement tpcm = other.gameObject.GetComponent<ThirdPersonCharacterMovement>();
            BeetleFlight bf = other.gameObject.GetComponent<BeetleFlight>();

            if(bf != null)
            {
                bf.StopAllMovement();
            }

            if(tpcm){
                tpcm.characterModel.SetActive(false);
                tpcm.enabled = false;
            }

            GameManager.Instance.SetDestinationPlayerStart(destinationPlayerStart);
            GameManager.Instance.LoadAfterDelay(0.5f, this.destinationScene);
        }
    }
}
