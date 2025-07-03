using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Allows the user to create a specified number of clones of a provided object in the Editor.
/// </summary>
public class EditorCreateObjects : MonoBehaviour {

    //Provided object of which to clone
    public GameObject objectToCreate;

    //How many clones do we wish to create?
    public int numberOfObjectToCreate = 1;

    //What is the spacing between each cloned object?
    public Vector3 offsetBetweenObjects = Vector3.forward;

    //Stored list of all clones. This is used when we with to delete all objects we've previously created.
    //Used when creating new clones also, as the previously created objects should be deleted.
    private List<GameObject> allInstantiatedObjects;

    //Do we want to start these objects active? Useful if we're implementing custom scripts on the cloned objects for toggling active.
    public bool objectsActive = true;

    /// <summary>
    /// Instantiate clones of the object we wish top clone.
    /// </summary>
    public void CreateObjects()
    {
        if(allInstantiatedObjects == null)
        {
            allInstantiatedObjects = new List<GameObject>();
        }

        DestroyObjects();

        numberOfObjectToCreate--;

        for(int i = 0; i < numberOfObjectToCreate+1; i++)
        {
            GameObject currentObjectToCreate = Instantiate(objectToCreate);
            currentObjectToCreate.transform.parent = this.transform.parent;
            currentObjectToCreate.transform.localScale = this.transform.localScale;
            currentObjectToCreate.transform.position = this.transform.position + offsetBetweenObjects * (i+1);
            currentObjectToCreate.name = this.name + " " + (i+1);

            if(currentObjectToCreate.GetComponent<MouseUIInteractable>()!=null){
                currentObjectToCreate.GetComponent<MouseUIInteractable>().floorNumber = i + 1;
            }

            allInstantiatedObjects.Add(currentObjectToCreate);

            currentObjectToCreate.SetActive(objectsActive);
        }

        numberOfObjectToCreate++;
    }

    /// <summary>
    /// Removes previously created objects from the list stored earlier. WARNING; this will not save between sessions. When restarting Unity or reloading scenes the stored list of objects created will be forgotten.
    /// </summary>
    public void DestroyObjects()
    {
        if(allInstantiatedObjects.Count > 0)
        {
            foreach (GameObject go in allInstantiatedObjects)
            {
                if (go != null)
                {
                    DestroyImmediate(go);
                }
            }
        }

        //Re-initialize the list
        allInstantiatedObjects = new List<GameObject>();
    }
}
