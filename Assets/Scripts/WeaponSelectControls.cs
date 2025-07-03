using UnityEngine;

public class WeaponSelectControls : MonoBehaviour
{
    public WeaponButton[] weaponButtons;

    private void LateUpdate()
    {
        if (GameManager.Instance.GetIsPaused())
        {
			return;
        }

		if (GameManager.Instance.GetPlayerInputManager().GetLeftWeaponSelectPressed())
		{
			LeftSelect();
		}

		if (GameManager.Instance.GetPlayerInputManager().GetRightWeaponSelectPressed())
		{
			RightSelect();
		}
	}

    private void LeftSelect()
	{
		int desiredWeaponIndexAfterSelect = GameManager.Instance.currentWeaponIndex - 1;

		Select(desiredWeaponIndexAfterSelect, false);
	}

	private void RightSelect()
	{
		int desiredWeaponIndexAfterSelect = GameManager.Instance.currentWeaponIndex + 1;

		UnityEngine.Debug.Log("test");

		Select(desiredWeaponIndexAfterSelect, true);
	}

	private void Select(int index, bool goingup)
    { 
		//Check if index is in the bounds of the array
		//If not then loop back around to select the highest or lowest unlocked weapon

		if(index >= 0 && index < (GameManager.Instance.weaponsUnlocked.Length))
        {
            if (GameManager.Instance.weaponsUnlocked[index])
            {
                GameManager.Instance.currentWeaponIndex = index;
            } else
            {
                if (goingup)
                {
                    Select(index + 1, true);
                }
                else
                {
                    Select(index - 1, false);
                }
            }
        } else
        {
            //In this event the provided index is outside of the array bounds
            if (index < 0)
            {
                for (int i = GameManager.Instance.weaponsUnlocked.Length - 1; i > 0; i--)
                {
                    if (GameManager.Instance.weaponsUnlocked[i])
                    {
                        GameManager.Instance.currentWeaponIndex = i;
                        break;
                    }
                }
            }
            else
            {
                GameManager.Instance.currentWeaponIndex = 0;
            }

        }
    }
}
