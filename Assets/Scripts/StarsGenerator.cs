using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Will create a lot of objects in 3D space like stars.
/// </summary>
public class StarsGenerator : MonoBehaviour
{
    public enum StarGenerationType { starField, straightLine, tenDirectional }
    public StarGenerationType starGenerationType;

    public int starsToGenerate = 200;
    public float spaceBetweenStars = 3.14f;
    public float generationSpeedIntervals = 0.5f;

    [Header("Used only for starField generation type")]
    public float starFieldBounds = 4000f;

    [Header("Will use current object if nothing provided")]
    public GameObject star;

    private int starsRemainingToGenerate;

    private float currentStarSpacing;

    private List<Vector3> locationsOccupied;

    private void Awake()
    {
        if(star == null)
        {
            star = this.gameObject;
        }

        starsRemainingToGenerate = starsToGenerate;
        currentStarSpacing = spaceBetweenStars;

        locationsOccupied = new List<Vector3>();
    }

    private void Start()
    {
        InvokeRepeating("GenerateStars", 0.2f, generationSpeedIntervals);
    }

    private void GenerateStars()
    {
        if (starsRemainingToGenerate > 0)
        {
            if(starGenerationType == StarGenerationType.starField)
            {
                GameObject currentStar = CreateNewStar();

                Vector3 newStarLocation = Random.insideUnitSphere * starFieldBounds;

                if (!locationsOccupied.Contains(newStarLocation))
                {
                    currentStar.transform.position = newStarLocation;
                    locationsOccupied.Add(newStarLocation);
                }
            }

            if (starGenerationType == StarGenerationType.straightLine)
            {
                GameObject currentStar = CreateNewStar();
                currentStar.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + currentStarSpacing);
            }

            if (starGenerationType == StarGenerationType.tenDirectional)
            {
                #region horizontalAxisStars
                GameObject currentStar = CreateNewStar();
                currentStar.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + currentStarSpacing);

                currentStar = CreateNewStar();
                currentStar.transform.position = new Vector3(transform.position.x + currentStarSpacing, transform.position.y, transform.position.z);

                currentStar = CreateNewStar();
                currentStar.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - currentStarSpacing);

                currentStar = CreateNewStar();
                currentStar.transform.position = new Vector3(transform.position.x - currentStarSpacing, transform.position.y, transform.position.z);
                #endregion

                #region verticalAxisStars
                currentStar = CreateNewStar();
                currentStar.transform.position = new Vector3(transform.position.x, transform.position.y + currentStarSpacing, transform.position.z);

                currentStar = CreateNewStar();
                currentStar.transform.position = new Vector3(transform.position.x, transform.position.y - currentStarSpacing, transform.position.z);
                #endregion

                #region diagonalStarsHorizontal
                currentStar = CreateNewStar();
                currentStar.transform.position = new Vector3(transform.position.x + currentStarSpacing, transform.position.y, transform.position.z + currentStarSpacing);

                currentStar = CreateNewStar();
                currentStar.transform.position = new Vector3(transform.position.x - currentStarSpacing, transform.position.y, transform.position.z - currentStarSpacing);

                currentStar = CreateNewStar();
                currentStar.transform.position = new Vector3(transform.position.x + currentStarSpacing, transform.position.y, transform.position.z - currentStarSpacing);

                currentStar = CreateNewStar();
                currentStar.transform.position = new Vector3(transform.position.x - currentStarSpacing, transform.position.y, transform.position.z + currentStarSpacing);

                #endregion

                /*
                #region diagonalStarsVertical
                currentStar = CreateNewStar();
                currentStar.transform.position = new Vector3(transform.position.x + currentStarSpacing, transform.position.y + currentStarSpacing, transform.position.z + currentStarSpacing);

                currentStar = CreateNewStar();
                currentStar.transform.position = new Vector3(transform.position.x - currentStarSpacing, transform.position.y, transform.position.z - currentStarSpacing);

                currentStar = CreateNewStar();
                currentStar.transform.position = new Vector3(transform.position.x + currentStarSpacing, transform.position.y, transform.position.z - currentStarSpacing);

                currentStar = CreateNewStar();
                currentStar.transform.position = new Vector3(transform.position.x - currentStarSpacing, transform.position.y, transform.position.z + currentStarSpacing);

                #endregion
                */
            }
            currentStarSpacing += spaceBetweenStars;

        } else
        {
            CancelInvoke("GenerateStars");
        }
    }

    private GameObject CreateNewStar()
    {
        if (starsRemainingToGenerate > 0)
        {
            GameObject newStar = Instantiate(star);

            if (newStar.GetComponent<StarsGenerator>() != null)
            {
                newStar.GetComponent<StarsGenerator>().enabled = false;
            }

            starsRemainingToGenerate--;
            return newStar;
        } else
        {
            CancelInvoke("GenerateStars");
            return null;
        }
    }
}
