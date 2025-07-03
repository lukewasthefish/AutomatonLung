using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class DeleteSaveAction : MonoBehaviour
{
    public Transform redSquareVisualEffect;
    public NumberMesh saveFileIndexReference;

    public NumberMeshDisplaySaveFileChipCount numberMeshToUpdateAfterDeletion;

    private Vector3 initialRedSquareScale;

    private int numberOfTimesPressed = 0;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = this.GetComponent<AudioSource>();

        initialRedSquareScale = redSquareVisualEffect.transform.localScale;
        redSquareVisualEffect.transform.localScale = new Vector3(redSquareVisualEffect.transform.localScale.x, 0f, redSquareVisualEffect.transform.localScale.z);
    }

    public void DeletePressedAction()
    {
        redSquareVisualEffect.transform.localScale += new Vector3(0f, initialRedSquareScale.y * (1f / 8f), 0f);
        redSquareVisualEffect.GetComponent<MeshFlash>().Flash(0.2f);

        numberOfTimesPressed++;

        if(numberOfTimesPressed >= 8)
        {
            audioSource.Play();
            redSquareVisualEffect.transform.localScale = new Vector3(redSquareVisualEffect.transform.localScale.x, 0f, redSquareVisualEffect.transform.localScale.z);

            SaveFileUtils.DeleteSaveFile("saves/save" + saveFileIndexReference.numberToDisplay.ToString());

            numberOfTimesPressed = 0;

            numberMeshToUpdateAfterDeletion.UpdateNumber();
        }
    }
}
