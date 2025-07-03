using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This class recenters the whatever object this is attached to at (0, 0, 0) in world position when transform exits bounds.
/// Used to prevent strange floating point errors and allow for extremely large worlds.
/// </summary>
public class OpenWorldRecentering : MonoBehaviour {

    public float bounds = 500f; //How many units would this object have to travel to incurr a world recenter?
    public float externalBounds = 1500f; //How many units would this object have to travel to loop around the world?

    private Transform mainCamera;

    private Vector3 distanceTraveledFromStart = Vector3.zero;

    private List<GameObject> allOtherGameObjects = new List<GameObject>(); //All gameObjects aside from the one currently utilizing this class

    private BeetleFlight bf;

    private void Awake()
    {
        bf = this.GetComponent<BeetleFlight>();
        mainCamera = Camera.main.transform;

        allOtherGameObjects.AddRange(SceneManager.GetActiveScene().GetRootGameObjects());
        allOtherGameObjects.Remove(this.gameObject);
        allOtherGameObjects.Remove(mainCamera.gameObject);
    }

    private void LateUpdate()
    {
        if(Mathf.Max(Mathf.Abs(distanceTraveledFromStart.x), Mathf.Abs(distanceTraveledFromStart.y), Mathf.Abs(distanceTraveledFromStart.z)) >= externalBounds){
            //Loop around the world
            WorldLoop();
        }

        if(Mathf.Max(Mathf.Abs(this.transform.position.x),Mathf.Abs(this.transform.position.y),Mathf.Abs(this.transform.position.z)) >= bounds)
        {
            distanceTraveledFromStart += Recenter();
        }
    }

    public Vector3 Recenter()
    {
        if(this.bf != null)
        {
            bf.ResetTrails();
        }

        Vector3 vectorFromEarlierPositionToHere = this.transform.position;

        //TODO : Actually implement threading (just for optimization)
        //START OF THREADING
        foreach (GameObject go in allOtherGameObjects)
        {
            if(go && go.transform)
            {
                MoveTransform(go.transform, -vectorFromEarlierPositionToHere);
            }
        }
        //END OF THREADING

        mainCamera.transform.parent = this.gameObject.transform;
        this.transform.position = Vector3.zero;
        mainCamera.transform.parent = null;

        if (this.bf != null)
        {
            bf.ResetTrails();
        }

        return vectorFromEarlierPositionToHere;
    }

    private void MoveTransform(Transform transformToMove, Vector3 movementOffset)
    {
        transformToMove.position += movementOffset;
    }

    private void WorldLoop(){

        if (this.bf != null)
        {
            bf.ResetTrails();
        }

        mainCamera.transform.parent = this.gameObject.transform;

        this.transform.position = new Vector3(-distanceTraveledFromStart.x*2f, this.transform.position.y, -distanceTraveledFromStart.z*2f) / 1.05f;

        mainCamera.transform.parent = null;

        if(SteamManager.Initialized)
        {
            Steamworks.SteamUserStats.SetAchievement("Achievement_FlyAroundTheWorld");
            Steamworks.SteamUserStats.StoreStats();
        }
    }
}

public struct MoveTransformJob : IJob
{
    public void Execute()
    {

    }
}

public static class Utility
{
    public static void Invoke(this MonoBehaviour mb, Action f, float delay)
    {
        mb.StartCoroutine(InvokeRoutine(f, delay));
    }

    private static IEnumerator InvokeRoutine(Action f, float delay)
    {
        yield return new WaitForSeconds(delay);
        f();
    }
}