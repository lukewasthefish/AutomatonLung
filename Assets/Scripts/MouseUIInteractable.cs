using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// TODO : Refactor OR replace this entire class with our new menu system (as seen in the pause menu)
/// Any object with this script attached can be interacted with by touching it on the LowerLCD screen or clicking on it in the editor.
/// Used as a replacement for most UI elements that normally would need to use a canvas object, such as buttons.
/// </summary>
public class MouseUIInteractable : MonoBehaviour {

    public enum ActionType {
        Destroy,
        LoadScene,
        CameraZoomChange,
        WeaponSwitch,
        DeleteSaveFile,
        ToggleMusic,
        ToggleCameraInverted,
        QuitApplication
    }

    public ActionType actionType = ActionType.Destroy;
    public AudioSource audioSource;

    [Header("Elevator buttons will not use destination scene, but will instead load by this object name.")]
    public bool isElevatorButton = false;
    public bool isTitleScreenButton = false;

    [Header("What scene to load?")]
    public string destinationScene;

    [Header("Value used when action deals with a value")]
    public Camera cameraToChange;
    public float changeAmmount;
    public float zoomLimit = 2000f;

    public int floorNumber = 0;
    public int playerStart = 0;
    private int numberOfTimesPressed = 0;

    [Header("Actions on button depend on action type.")]
    public GameObject objectToActOn;

    private Vector3 initialObjectToDestroyScale = Vector3.one / 100f;

    private GameObject scrollingBG;

    private void OnMouseDown()
    {
        Press();
    }

    public void Press()
    {
        ButtonAction();
        numberOfTimesPressed++;
    }

    private void Awake()
    {
        scrollingBG = GameObject.Find("ScrollingBG");
    }

    private void Start(){
        //if(scrollingBG)
        //scrollingBG.GetComponent<ScrollingTexture>().enabled = false;

        if (actionType == ActionType.DeleteSaveFile){
            initialObjectToDestroyScale = objectToActOn.transform.localScale;
            objectToActOn.transform.localScale = new Vector3(objectToActOn.transform.localScale.x, 0f, objectToActOn.transform.localScale.z);
        }
    }

    private void ButtonLoadScene()
    {
        GameManager.Instance.SetDestinationPlayerStart(-1);

        if (isElevatorButton)
        {
            GameManager.Instance.SetDestinationPlayerStart(0);
            Camera.main.backgroundColor = Color.black;
            LoadSceneFromElevatorButton();
        }
        else
        {

            if (isTitleScreenButton && GameManager.Instance.CurrentSaveData.ContainsKey("saveScene"))
            {
                destinationScene = GameManager.Instance.CurrentSaveData.GetString("saveScene");
            }

            if (isTitleScreenButton && !GameManager.Instance.CurrentSaveData.ContainsKey("saveScene"))
            {
                GameManager.Instance.SetDestinationPlayerStart(-2);
            }
            GameManager.Instance.LoadSceneImmediate(destinationScene);
            return;
        }
    }

    public virtual void ButtonAction(){
        if(actionType == ActionType.QuitApplication)
        {
            Application.Quit();
            return;
        }

        if(actionType == ActionType.Destroy)
        {
            this.gameObject.SetActive(false);
        }

        if (actionType == ActionType.LoadScene)
        {
            if (SceneManager.GetActiveScene().name == "Elevator")
            {
                float timeToInvoke = 6f;

                GameObject elevatorUI = GameObject.Find("ElevatorUI");
                GameObject upperLCDCamera = GameObject.Find("UpperLCDCamera");
                if (elevatorUI)
                {
                    elevatorUI.SetActive(false);
                }

                if (upperLCDCamera)
                {
                    Animator upperLCDCameraAnimator = upperLCDCamera.GetComponent<Animator>();
                    upperLCDCameraAnimator.SetBool("transition", true);
                    bool goingup = GameManager.Instance.previousElevatorFloorNumber <= floorNumber ? true : false;

                    if(GameManager.Instance.previousElevatorFloorNumber == floorNumber)
                    {
                        timeToInvoke = 1f;
                        upperLCDCameraAnimator.SetBool("transition", false);
                    }

                    if (goingup)
                    {
                        //scrollingBG.GetComponent<ScrollingTexture>().scrollSpeedY *= -1f;
                    }

                    upperLCDCameraAnimator.SetBool("goingup", goingup);
                }

                Invoke("ButtonLoadScene", timeToInvoke);

                return;
            }

            ButtonLoadScene();
        }
    }

    private void LoadSceneFromElevatorButton()
    {
        Debug.Log("Floor " + floorNumber + " with Player Start of : " + playerStart);

        GameManager.Instance.SetDestinationPlayerStart(playerStart);
        GameManager.Instance.LoadSceneImmediate("Floor " + floorNumber);
    }
}
