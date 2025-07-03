using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

/// <summary>
/// Lists all objects of the current scene in some text 
/// </summary>
public class ListObjects : MonoBehaviour
{
    public TextMeshPro allSceneObjectsText;
    public TextMeshPro currentSelectedObjectText;

    private string displayString;

    private List<GameObject> sceneObjects = new List<GameObject>();
    private GameObject currentSelectedObject;

    private int previousFrameObjectCount = 0;
    private void Update()
    {
        int currentFrameObjectCount = SceneManager.GetActiveScene().rootCount;
        if (currentSelectedObject)
        {
            currentSelectedObjectText.text = currentSelectedObject.name;
        }

        if (currentFrameObjectCount != previousFrameObjectCount)
        {
            UpdateObjectsList();
            UpdateText();
        }

        //Select a new random objects
        if (GameManager.Instance.GetPlayerInputManager().GetRightWeaponSelectPressed())
        {
            currentSelectedObject = sceneObjects[Random.Range(0, sceneObjects.Count)];
        }

        //destroy the currently selected objects
        if (GameManager.Instance.GetPlayerInputManager().GetRightWeaponSelectHeld() && GameManager.Instance.GetPlayerInputManager().GetLeftWeaponSelectHeld())
        {
            Destroy(currentSelectedObject);
            //currentSelectedObject.SetActive(false);
        }

        previousFrameObjectCount = currentFrameObjectCount;
    }

    private void UpdateObjectsList()
    {
        sceneObjects.Clear();
        foreach(GameObject go in SceneManager.GetActiveScene().GetRootGameObjects())
        {
            foreach(Transform t in go.GetComponentsInChildren<Transform>(true))
            {
                sceneObjects.Add(t.gameObject);
            }
        }
    }

    private void UpdateText()
    {
        displayString = "";

        foreach(GameObject go in sceneObjects)
        {
            displayString += go.name + "\n";
        }

        allSceneObjectsText.text = displayString;
    }
}
