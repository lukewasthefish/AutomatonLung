using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayAxisViaNumberMesh : MonoBehaviour
{
	private NumberMesh numberMesh;

	public enum AxisToDisplay { FireAxis, LockonAxis}
	public AxisToDisplay axisToDisplay = AxisToDisplay.FireAxis;

	private void Awake()
	{
		numberMesh = GetComponent<NumberMesh>();
	}

	private void Update()
	{
		int displayNumber1 = 0;
		int displayNumber2 = 0;

        if (GameManager.Instance.GetPlayerInputManager().GetLockOnHeld())
        {
			displayNumber1 = 1;
		}

		if (GameManager.Instance.GetPlayerInputManager().GetFireHeld())
		{
			displayNumber2 = 1;
		}

		switch (axisToDisplay)
        {
			case AxisToDisplay.FireAxis:
				numberMesh.numberToDisplay = displayNumber1;
				break;
			case AxisToDisplay.LockonAxis:
				numberMesh.numberToDisplay = displayNumber2;
				break;
        }
	}
}
