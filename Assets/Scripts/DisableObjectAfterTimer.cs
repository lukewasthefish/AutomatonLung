using UnityEngine;

public class DisableObjectAfterTimer : MonoBehaviour
{
    public float time = 3f;
    private float currentTime = 0f;
    private void Update()
    {
        if(currentTime >= time)
        {
            this.gameObject.SetActive(false);
            return;
        }

        currentTime += Time.deltaTime;
    }
}
