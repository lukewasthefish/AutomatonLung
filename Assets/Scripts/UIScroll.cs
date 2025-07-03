using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.N3DS;

/// <summary>
/// UIScroll element controlled by player that limits the transform of the required RectTransform component within a range.
/// The player may use the input axis to control this scrolling element.
/// </summary>
[RequireComponent(typeof(RectTransform))]
public class UIScroll : MonoBehaviour {

    public float limit = 1200f;

    public float moveSpeed = 5f;

    private float initialHeight;    //Get initial height at which to adjust the limit from.

    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();

        initialHeight = rectTransform.position.y;

        limit /= 2f;
    }

    public void Move(float moveAmmount){
        transform.position = new Vector3(transform.position.x, transform.position.y + moveAmmount, transform.position.z);
    }

    private void LateUpdate(){
        
        this.transform.position = new Vector3(transform.position.x, transform.position.y - GameManager.Instance.GetPlayerInputManager().GetLeftStick().y * moveSpeed * Time.deltaTime, transform.position.z);

        while(rectTransform.position.y < -limit + initialHeight)
        {
            transform.position -= Vector3.down / 1000f;
        }

        while(rectTransform.position.y > limit + initialHeight)
        {
            transform.position += Vector3.down / 1000f;
        }
    }
}
