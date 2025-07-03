using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponButton : MouseUIInteractable {
	public int weaponIndex = 0;

	private PlayerFire playerFire; 

	private void Awake(){
		this.playerFire = FindObjectOfType<PlayerFire>();
	}

	private void Start()
	{
		if(GameManager.Instance.currentWeaponIndex == weaponIndex){
			ButtonAction();
		}

		string key = "weaponUnlocked" + weaponIndex;
		if(GameManager.Instance.CurrentSaveData != null && GameManager.Instance.CurrentSaveData.GetBool(key)){
			GameManager.Instance.weaponsUnlocked[weaponIndex] = true;
		}

		this.GetComponent<Renderer>().enabled = false;
	}

	public void UpdateWidth()
	{
		//Button state when active selected weapon
		if (GameManager.Instance.currentWeaponIndex == this.weaponIndex)
        {
			this.transform.localScale = new Vector3(2f, transform.localScale.y, transform.localScale.z);
		} else
        {
			//Button state when NOT active selected weapon
			this.transform.localScale = new Vector3(0.85f, transform.localScale.y, transform.localScale.z);
		}
	}

    public override void ButtonAction()
    {
        GameManager.Instance.currentWeaponIndex = this.weaponIndex;
    }

	public void SelectWeapon(int weaponIndex)
    {
		GameManager.Instance.currentWeaponIndex = weaponIndex;
	}
}
