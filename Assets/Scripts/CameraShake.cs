using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private Transform cameraTransform;
    private float shakeTimeRemaining = 0f;
    private float currentShakeStrength = 0.8f;
    private float currentTimeRemainingMax = 1f; //Do not divide by zero

    private void Awake(){
        if(!cameraTransform){
            cameraTransform = this.transform;
        }
    }

    /// <summary>
    /// Wait and act upon input from the Shake method in this same class. 
    /// </summary>
    public void ReadShake(){
        if(shakeTimeRemaining >= 0f){
            cameraTransform.transform.position += Random.insideUnitSphere * Time.deltaTime * 2f * currentShakeStrength * (shakeTimeRemaining / currentTimeRemainingMax);
            shakeTimeRemaining -= 1f * Time.deltaTime;
        }
    }

    /// <summary>
    /// Initializes shake
    /// </summary>
    /// <param name="duration"></param>
    /// <param name="strength"></param>
    public void Shake(float duration, float strength){
        currentShakeStrength = strength;
        shakeTimeRemaining = duration;
        currentTimeRemainingMax = duration;
    }
}