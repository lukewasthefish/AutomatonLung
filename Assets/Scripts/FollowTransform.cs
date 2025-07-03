using UnityEngine;

public class FollowTransform : MonoBehaviour
{
    public Transform transformToFollow;

    public bool lerp = false;
    public float lerpSpeed = 8f;

    public Vector3 worldOffset;

    private void Awake()
    {
        if (!transformToFollow)
        {
            UnityEngine.Debug.LogWarning("There is not transform to follow assigned on " + this.gameObject.name);
            return;
        }

    }

    private void Update()
    {
        if (!transformToFollow)
        {
            return;
        }

        Vector3 destination = transformToFollow.position + worldOffset;

        if (lerp)
        {
            this.transform.position = Vector3.Lerp(this.transform.position, destination, lerpSpeed * Time.deltaTime);
        } else
        {
            this.transform.position = destination;
        }
    }
}
