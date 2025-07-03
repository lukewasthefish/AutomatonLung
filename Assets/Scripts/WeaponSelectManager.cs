using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSelectManager : MonoBehaviour
{
    public WeaponButton[] weaponButtons;

	private int numberOfUnlockedWeapons = 1;

	private void Update()
    {
		CountNumberOfUnlockedWeapons();
    }

    private void CountNumberOfUnlockedWeapons()
	{
		numberOfUnlockedWeapons = 0;

		for (int i = 0; i < weaponButtons.Length; i++)
		{
			weaponButtons[i].UpdateWidth();

			if (GameManager.Instance.weaponsUnlocked[i])
			{
				numberOfUnlockedWeapons++;
			}
		}

		for(int i = 0; i < weaponButtons.Length; i++)
        {
			if (numberOfUnlockedWeapons > 1 && GameManager.Instance.weaponsUnlocked[i])
			{
				weaponButtons[i].GetComponent<Renderer>().enabled = true;
			}
		}
	}
}
