// Cinema Suite
using UnityEngine;
using UnityEngine.UI;

namespace CinemaDirector
{
    /// <summary>
    /// An action that fades in a texture over the first 25% of length, shows for 50% of time length
    /// and fades away over the final 25%.
    /// </summary>
    [CutsceneItem("Transitions", "Fade Texture", CutsceneItemGenre.GlobalItem)]
    public class CanvasFadeTexture : CinemaGlobalAction
    {
        public Sprite sprite;
        [Tooltip("The width the want the sprite to be.")]
        public int width = 100;
        [Tooltip("The height the want the sprite to be.")]
        public int height = 100;

        // Optional Tint
        public Color tint = Color.grey;

        private GameObject canvasGO;
        private RectTransform canvas;
        private RectTransform imageRect;
        private Image image;
        private string prefabName = "Transition Canvas";

        public void Setup()
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

            // Set image
            canvas = canvasGO.GetComponent<RectTransform>();
            image = canvasGO.transform.GetChild(0).GetComponent<Image>();
            image.sprite = sprite;

            // Set image to start color
            image.color = tint;

            imageRect = canvasGO.transform.GetChild(0).GetComponent<RectTransform>();
            imageRect.sizeDelta = new Vector2(width, height);

            // Disable image
            canvas.gameObject.SetActive(false);
        }

        /// <summary>
        /// Disable the Texture and make it clear.
        /// </summary>
        void Awake()
        {
            Setup();
        }

        /// <summary>
        /// Trigger this event, enable the texture and make it clear.
        /// </summary>
        public override void Trigger()
        {
            Setup();
            if (canvas)
            {
                canvas.gameObject.SetActive(true);
            }
        }

        /// <summary>
        /// Reverse the start of this action by disabling the texture.
        /// </summary>
        public override void ReverseTrigger()
        {
            End();
        }

        /// <summary>
        /// Update the fading/showing of this texture.
        /// </summary>
        /// <param name="time">The time of this action.</param>
        /// <param name="deltaTime">The deltaTime since last update.</param>
        public override void UpdateTime(float time, float deltaTime)
        {
            if (canvas)
            {
                float transition = time / Duration;
                if (transition <= 0.25f)
                {
                    FadeToColor(Color.clear, tint, (transition / 0.25f));
                }
                else if (transition >= 0.75f)
                {
                    FadeToColor(tint, Color.clear, (transition - 0.75f) / .25f);
                }
            }
        }

        /// <summary>
        /// Update this action to an arbitrary time.
        /// </summary>
        /// <param name="time">The new time.</param>
        /// <param name="deltaTime">The deltaTime since last update.</param>
        public override void SetTime(float time, float deltaTime)
        {
            if (canvas)
            {
                canvas.gameObject.SetActive(true);
                if (time >= 0 && time <= Duration)
                {
                    UpdateTime(time, deltaTime);
                }
                else if (canvas.gameObject.activeSelf)
                {
                    canvas.gameObject.SetActive(false);
                }
            }
        }

        /// <summary>
        /// End this action and disable the texture.
        /// </summary>
        public override void End()
        {
            if (canvas)
            {
                canvas.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// Trigger the action from the end in reverse.
        /// </summary>
        public override void ReverseEnd()
        {
            Trigger();
        }

        /// <summary>
        /// Stop this action and disable its texture.
        /// </summary>
        public override void Stop()
        {
            if (canvas)
            {
                canvas.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// Fade between two colours over a transition value.
        /// </summary>
        /// <param name="from">The start color.</param>
        /// <param name="to">The end color.</param>
        /// <param name="transition">The transition amount.</param>
        private void FadeToColor(Color from, Color to, float transition)
        {
            if (image)
            {
                image.GetComponent<Image>().color = Color.Lerp(from, to, transition);
            }
        }
    }
}