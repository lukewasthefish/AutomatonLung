using UnityEngine;
using UnityEngine.UI;

namespace CinemaDirector
{

    /// <summary>
    /// Generic fade to white.
    /// </summary>
    [CutsceneItem("Transitions", "Fade To White", CutsceneItemGenre.GlobalItem)]
    public class CanvasFadeToWhite : CinemaGlobalAction
    {
        private Color startColor = Color.clear;
        private Color endColor = Color.white;

        private GameObject canvasGO;
        private RectTransform canvas;
        private RectTransform image;
        private string prefabName = "Transition Canvas";

        /// <summary>
        /// Setup the effect when the script is loaded.
        /// </summary>
        void Awake()
        {
            if (!this.transform.Find(prefabName + "(Clone)"))
            {
                // Add canvas image to scene
                canvasGO = Instantiate(Resources.Load(prefabName, typeof(GameObject))) as GameObject;
            }
            else
            {
                canvasGO = this.transform.Find(prefabName + "(Clone)").gameObject;
            }

            canvasGO.transform.SetParent(this.transform);

            // Set image size
            canvas = canvasGO.GetComponent<RectTransform>();
            image = canvasGO.transform.GetChild(0).GetComponent<RectTransform>();
            image.sizeDelta = new Vector2(canvas.rect.width, canvas.rect.height);

            // Set image to start color
            image.GetComponent<Image>().color = startColor;

            // Disable image
            canvas.gameObject.SetActive(false);
        }

        public override void Trigger()
        {
            if (canvas && image)
            {
                canvas.gameObject.SetActive(true);
                image.sizeDelta = new Vector2(canvas.rect.width, canvas.rect.height);
                image.GetComponent<Image>().color = startColor;
            }
        }

        public override void ReverseTrigger()
        {
            End();
        }

        public override void End()
        {
            if (canvas)
            {
                canvas.gameObject.SetActive(false);
            }
        }

        public override void ReverseEnd()
        {
            if (canvas && image)
            {
                canvas.gameObject.SetActive(true);
                image.sizeDelta = new Vector2(canvas.rect.width, canvas.rect.height);
                image.GetComponent<Image>().color = endColor;
            }
        }

        public override void Stop()
        {
            End();
        }

        public override void UpdateTime(float time, float deltaTime)
        {
            float transition = time / Duration;
            FadeToColor(startColor, endColor, transition);
        }

        public override void SetTime(float time, float deltaTime)
        {
            if (canvas)
            {
                if (time >= 0 && time <= Duration)
                {
                    canvas.gameObject.SetActive(true);
                    UpdateTime(time, deltaTime);
                }
                else if (image.gameObject.activeSelf)
                {
                    canvas.gameObject.SetActive(false);
                }
            }
        }

        /// <summary>
        /// Fade from one colour to another over a transition period.
        /// </summary>
        /// <param name="from">The starting colour</param>
        /// <param name="to">The final colour</param>
        /// <param name="transition">the Lerp transition value</param>
        private void FadeToColor(Color start, Color end, float transition)
        {
            if (image)
            {
                image.GetComponent<Image>().color = Color.Lerp(start, end, transition);
            }
        }
    }
}