using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Handles InputSystem UnityEvents
/// </summary>
public class InputManager : MonoBehaviour
{
    [SerializeField]
    private InputActionAsset inputActions;

    private PlayerInput playerInput;

    //Pressed = was button or trigger activated this frame
    //Held = currently being held down by user
    //Released = pressure on button or trigger just released this frame

    //Refers to the 'confirm' action button
    private bool confirmPressed = false;
    private bool confirmHeld = false;
    private bool confirmReleased = false;

    private bool firePressed = false;
    private bool fireHeld = false;
    private bool fireReleased = false;

    private bool boostPressed = false;
    private bool boostHeld = false;
    private bool boostReleased = false;

    private bool jumpPressed = false;
    private bool jumpHeld = false;
    private bool jumpReleased = false;

    private bool hoverboardPressed = false;
    private bool hoverboardHeld = false;
    private bool hoverboardReleased = false;

    private bool cheatCodeModifierPressed;
    private bool cheatCodeModifierHeld;
    private bool cheatCodeModifierReleased;

    private bool lockOnPressed = false;
    private bool LockOnHeld = false;
    private bool LockOnReleased = false;

    private bool pausePressed = false;
    private bool pauseHeld = false;
    private bool pauseReleased = false;

    private bool rightWeaponSelectPressed = false;
    private bool rightWeaponSelectHeld = false;
    private bool rightWeaponSelectReleased = false;

    private bool leftWeaponSelectPressed = false;
    private bool leftWeaponSelectHeld = false;
    private bool leftWeaponSelectReleased = false;

    private bool leftStickAtRestLastFrame = true;

    //The gamepad sticks (or the keyboard equivalent)
    private Vector2 leftStick;
    private Vector2 rightStick;

    private int resetInt = 0;

    private float stickMenuDeadzone = 0.3f;

    private InputActionMap mostRecentActionMap;
    private InputActionRebindingExtensions.RebindingOperation inputActionRebindingOp;

    private void Awake()
    {
        playerInput = this.GetComponent<PlayerInput>();

        if (!playerInput) 
        {
            playerInput = FindObjectOfType<PlayerInput>();
        }

        mostRecentActionMap = playerInput.currentActionMap;
    }

    public InputActionAsset GetInputActionAsset()
    {
        return inputActions;
    }

    private void Update()
    {
        ResetButtonsIfRecentButtonAction();
        UpdateActiveMap();
    }

    private void LateUpdate()
    {
        if (Mathf.Abs(GetLeftStick().x) > stickMenuDeadzone || Mathf.Abs(GetLeftStick().y) > stickMenuDeadzone)
        {
            leftStickAtRestLastFrame = false;
        } else
        {
            leftStickAtRestLastFrame = true;
        }
    }

    private void UpdateActiveMap()
    {
        bool useMenuControls = GameManager.Instance.GetIsPaused() || (!GameManager.Instance.thirdPersonCharacterMovement && !GameManager.Instance.beetleFlight);
        if (useMenuControls)
        {
            if (mostRecentActionMap.name != "Menu")
            {
                playerInput.SwitchCurrentActionMap("Menu");
                mostRecentActionMap = playerInput.currentActionMap;
            }
        }
        else
        {
            if (mostRecentActionMap.name != "Gameplay")
            {
                playerInput.SwitchCurrentActionMap("Gameplay");
                mostRecentActionMap = playerInput.currentActionMap;
            }
        }
    }

    private void ResetButtonsIfRecentButtonAction()
    {
        if (resetInt == 2)
        {
            ResetRecentButtons();
            resetInt = 0;
        }

        if (RecentButtonAction())
        {
            resetInt = 1;
        }

        if (resetInt == 0)
        {
            return;
        }

        if (resetInt == 1)
        {
            resetInt = 2;
            return;
        }
    }

    private void ResetRecentButtons()
    {
        if (firePressed)
        {
            firePressed = false;
        }

        if (fireReleased)
        {
            fireReleased = false;
        }

        if (confirmPressed)
        {
            confirmPressed = false;
        }

        if (confirmReleased)
        {
            confirmReleased = false;
        }

        if (jumpPressed)
        {
            jumpPressed = false;
        }

        if (jumpReleased)
        {
            jumpReleased = false;
        }

        if (boostPressed)
        {
            boostPressed = false;
        }

        if (boostReleased)
        {
            boostReleased = false;
        }

        if (hoverboardPressed)
        {
            hoverboardPressed = false;
        }

        if (hoverboardReleased)
        {
            hoverboardReleased = false;
        }

        if(cheatCodeModifierPressed)
        {
            cheatCodeModifierPressed = false;
        }

        if(cheatCodeModifierReleased)
        {
            cheatCodeModifierReleased = false;
        }

        if (pausePressed)
        {
            pausePressed = false;
        }

        if (pauseReleased)
        {
            pauseReleased = false;
        }

        if (rightWeaponSelectPressed)
        {
            rightWeaponSelectPressed = false;
        }

        if (rightWeaponSelectReleased)
        {
            rightWeaponSelectReleased = false;
        }

        if (leftWeaponSelectPressed)
        {
            leftWeaponSelectPressed = false;
        }

        if (leftWeaponSelectReleased)
        {
            leftWeaponSelectReleased = false;
        }

        if (lockOnPressed)
        {
            lockOnPressed = false;
        }

        if (LockOnReleased)
        {
            LockOnReleased = false;
        }
    }

    private bool RecentButtonAction()
    {
        //Returns if any of the below button actions have happened
        return
        firePressed
        ||
        fireReleased
        ||
        confirmPressed
        ||
        confirmReleased
        ||
        boostPressed
        ||
        boostReleased
        ||
        jumpPressed
        ||
        jumpReleased
        ||
        hoverboardPressed
        ||
        hoverboardReleased
        ||
        cheatCodeModifierPressed
        ||
        cheatCodeModifierReleased
        ||
        pausePressed
        ||
        pauseReleased
        ||
        rightWeaponSelectPressed
        ||
        rightWeaponSelectReleased
        ||
        leftWeaponSelectPressed
        ||
        leftWeaponSelectReleased
        ||
        lockOnPressed
        ||
        LockOnReleased;
    }

    private const int MAX_BINDINGS_PER_ACTION = 10;
    public void RebindAction(string inputActionName, GameObject[] showOnlyWhileBindingControl = null)
    {
        if (inputActionRebindingOp != null)
            inputActionRebindingOp.Dispose();

        if (showOnlyWhileBindingControl != null)
        {
            foreach (GameObject go in showOnlyWhileBindingControl)
            {
                go.SetActive(true);
            }
        }

        InputAction inputActionToRebind = inputActions.FindAction(inputActionName);

        if(inputActionToRebind.bindings.Count >= MAX_BINDINGS_PER_ACTION)
        {
            UnityEngine.Debug.LogWarning("This is the maximum number of bindings allowed per action");
            RebindComplete(inputActionToRebind, showOnlyWhileBindingControl);
            return;
        }

        UnityEngine.Debug.Log("Started rebinding");
        inputActionRebindingOp = inputActionToRebind.PerformInteractiveRebinding().WithRebindAddingNewBinding().OnComplete(operation => RebindComplete(inputActionToRebind, showOnlyWhileBindingControl)).Start();
    }

    private void RebindComplete(InputAction inputAction, GameObject[] showOnlyWhileBindingControl = null)
    {
        if (showOnlyWhileBindingControl != null)
        {
            foreach (GameObject go in showOnlyWhileBindingControl)
            {
                go.SetActive(false);
            }
        }

        RemoveDuplicateBindings(inputAction);

        inputActionRebindingOp.Dispose();
        ControlBindingsFileSaver.SaveControlBindings();
    }

    private void RemoveDuplicateBindings(InputAction inputAction)
    {
        List<string> paths = new List<string>();

        for(int i = 0; i < inputAction.bindings.Count; i++)
        {
            if (paths.Contains(inputAction.bindings[i].path))
            {
                inputAction.ChangeBinding(i).Erase();
            } else
            {
                paths.Add(inputAction.bindings[i].path);
            }
        }
    }

    public void ClearBindingsForAction(string actionName)
    {
        InputAction actionToRebindFromLoad = inputActions.FindActionMap("Gameplay").FindAction(actionName);

        while(actionToRebindFromLoad.bindings.Count > 0)
        {
            for (int i = 0; i < actionToRebindFromLoad.bindings.Count; i++)
            {
                actionToRebindFromLoad.ChangeBinding(i).Erase();
            }
        }
    }

    public bool GetConfirmPressed()
    {
        return confirmPressed;
    }

    public bool GetConfirmHeld()
    {
        return confirmHeld;
    }

    public bool GetConfirmReleased()
    {
        return confirmReleased;
    }

    public bool GetFirePressed()
    {
        return firePressed;
    }

    public bool GetFireHeld()
    {
        return fireHeld;
    }

    public bool GetFireReleased()
    {
        return fireReleased;
    }

    public bool GetBoostPressed()
    {
        return boostPressed;
    }

    public bool GetBoostHeld()
    {
        return boostHeld;
    }

    public bool GetBoostReleased()
    {
        return boostReleased;
    }

    public bool GetJumpPressed()
    {
        return jumpPressed;
    }

    public bool GetJumpHeld()
    {
        return jumpHeld;
    }

    public bool GetJumpReleased()
    {
        return jumpReleased;
    }

    public bool GetHoverboardPressed()
    {
        return hoverboardPressed;
    }

    public bool GetHoverboardHeld()
    {
        return hoverboardHeld;
    }

    public bool GetHoverboardReleased()
    {
        return hoverboardReleased;
    }

    public bool GetCheatCodeModifierPressed()
    {
        return cheatCodeModifierPressed;
    }

    public bool GetCheatCodeModifierHeld()
    {
        return cheatCodeModifierHeld;
    }

    public bool GetCheatCodeModifierReleased()
    {
        return cheatCodeModifierReleased;
    }

    public bool GetPausePressed()
    {
        return pausePressed;
    }

    public bool GetPauseHeld()
    {
        return pauseHeld;
    }

    public bool GetPauseReleased()
    {
        return pauseReleased;
    }

    public bool GetRightWeaponSelectPressed()
    {
        return rightWeaponSelectPressed;
    }

    public bool GetRightWeaponSelectHeld()
    {
        return rightWeaponSelectHeld;
    }

    public bool GetRightWeaponSelectReleased()
    {
        return rightWeaponSelectReleased;
    }

    public bool GetLeftWeaponSelectPressed()
    {
        return leftWeaponSelectPressed;
    }

    public bool GetLeftWeaponSelectHeld()
    {
        return leftWeaponSelectHeld;
    }

    public bool GetLeftWeaponSelectReleased()
    {
        return leftWeaponSelectReleased;
    }

    public bool GetLockOnPressed()
    {
        return lockOnPressed;
    }

    public bool GetLockOnHeld()
    {
        return LockOnHeld;
    }

    public bool GetLockOnReleased()
    {
        return LockOnReleased;
    }

    public bool GetMenuUpPressed()
    {
        if (leftStickAtRestLastFrame && leftStick.y > stickMenuDeadzone)
        {
            return true;
        }

        return false;
    }

    public bool GetMenuRightPressed()
    {
        if (leftStickAtRestLastFrame && leftStick.x > stickMenuDeadzone)
        {
            return true;
        }

        return false;
    }

    public bool GetMenuRightHeld()
    {
        return leftStick.x > stickMenuDeadzone;
    }

    public bool GetMenuDownPressed()
    {
        if (leftStickAtRestLastFrame && leftStick.y < -stickMenuDeadzone)
        {
            return true;
        }

        return false;
    }

    public bool GetMenuLeftPressed()
    {
        if (leftStickAtRestLastFrame && leftStick.x < -stickMenuDeadzone)
        {
            return true;
        }

        return false;
    }

    public bool GetMenuLeftHeld()
    {
        return leftStick.x < -stickMenuDeadzone;
    }

    public bool GetRightStickHeldRight()
    {
        if(rightStick.x > stickMenuDeadzone)
        {
            return true;
        }

        return false;
    }

    public Vector2 GetLeftStick()
    {
        return leftStick;
    }

    public Vector2 GetRightStick()
    {
        return rightStick;
    }

    public void Move(InputAction.CallbackContext context)
    {
        leftStick = context.ReadValue<Vector2>();
    }

    public void Look(InputAction.CallbackContext context)
    {
        rightStick = context.ReadValue<Vector2>();
        rightStick = new Vector2(Mathf.Clamp(rightStick.x, -5f, 5f), Mathf.Clamp(rightStick.y, -5f, 5f));
    }

    public void Boost(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            boostPressed = true;
            boostHeld = true;
        }

        if (context.canceled)
        {
            boostReleased = true;
            boostHeld = false;
        }
    }

    public void Hoverboard(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            hoverboardPressed = true;
            hoverboardHeld = true;
        }

        if (context.canceled)
        {
            hoverboardReleased = true;
            hoverboardHeld = false;
        }
    }

    public void CheatCodeModifier(InputAction.CallbackContext context)
    {
        if (context.started)
        {
           cheatCodeModifierPressed = true;
           cheatCodeModifierHeld = true; 
        }

        if(context.canceled)
        {
            cheatCodeModifierReleased = true;
            cheatCodeModifierHeld = false;
        }
    }

    public void LeftWeaponSelect(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            leftWeaponSelectPressed = true;
            leftWeaponSelectHeld = true;
        }

        if (context.canceled)
        {
            leftWeaponSelectReleased = true;
            leftWeaponSelectHeld = false;
        }
    }

    public void RightWeaponSelect(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            rightWeaponSelectPressed = true;
            rightWeaponSelectHeld = true;
        }

        if (context.canceled)
        {
            rightWeaponSelectReleased = true;
            rightWeaponSelectHeld = false;
        }
    }

    public void Pause(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            pausePressed = true;
            pauseHeld = true;
        }

        if (context.canceled)
        {
            pauseReleased = true;
            pauseHeld = false;
        }
    }

    public void Fire(InputAction.CallbackContext context)
    {
        //Fire pressed
        if (context.started)
        {
            firePressed = true;
            fireHeld = true;
        }

        //Fire released
        if (context.canceled)
        {
            fireReleased = true;
            fireHeld = false;
        }
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            jumpPressed = true;
            jumpHeld = true;
        }

        if (context.canceled)
        {
            jumpReleased = true;
            jumpHeld = false;
        }
    }

    public void LockOn(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            lockOnPressed = true;
            LockOnHeld = true;
        }

        if (context.canceled)
        {
            LockOnReleased = true;
            LockOnHeld = false;
        }
    }

    public void Confirm(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            confirmPressed = true;
            confirmHeld = true;
        }

        if (context.canceled)
        {
            confirmReleased = true;
            confirmHeld = false;
        }
    }
}
