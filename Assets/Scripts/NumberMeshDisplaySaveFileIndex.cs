using UnityEngine;

[RequireComponent(typeof(NumberMesh))]
public class NumberMeshDisplaySaveFileIndex : MonoBehaviour
{
    private NumberMesh numberMesh;

    private void Awake()
    {
        numberMesh = this.GetComponent<NumberMesh>();
    }

    public NumberMesh GetNumberMesh()
    {
        if (!numberMesh)
        {
            numberMesh = this.GetComponent<NumberMesh>();
        }

        return numberMesh;
    }
}
