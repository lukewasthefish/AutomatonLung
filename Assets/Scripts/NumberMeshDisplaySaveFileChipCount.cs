using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(NumberMesh))]
public class NumberMeshDisplaySaveFileChipCount : MonoBehaviour
{
    public int saveFileIndex = 1;
    private NumberMesh numberMesh;

    private void Awake()
    {
        numberMesh = this.GetComponent<NumberMesh>();
    }

    private void Start()
    {
        UpdateNumber();
    }

    public void UpdateNumber()
    {
        numberMesh.numberToDisplay = GameManager.Instance.GetChipCountFromSaveFileIndex(this.saveFileIndex);
    }
}
