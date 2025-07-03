using System.Collections;
using UnityEngine;

namespace CinemaDirector
{
    /// <summary>
    /// A sample behaviour for triggering Cutscenes.
    /// </summary>
    public class CutsceneTrigger : MonoBehaviour
    {
        public StartMethod StartMethod;
        public Cutscene Cutscene;
        public GameObject TriggerObject;
        public string SkipButtonName = "Jump";
        public string TriggerButtonName = "Fire1";
        public float Delay = 0f;
        private bool hasTriggered = false;

        /// <summary>
        /// When the trigger is loaded, optimize the Cutscene.
        /// </summary>
        void Awake()
        {
            if (Cutscene != null)
            {
                Cutscene.Optimize();
            }
        }

        // When the scene starts trigger the Cutscene if necessary.
        void Start()
        {
            if (StartMethod == StartMethod.OnStart && Cutscene != null)
            {
                hasTriggered = true;
                StartCoroutine(PlayCutscene());
            }
        }

        private IEnumerator PlayCutscene()
        {
            yield return new WaitForSeconds(Delay);
            this.Cutscene.Play();
        }

        void Update()
        {
            // Check if the user wants to skip.
            if (GameManager.Instance.GetPlayerInputManager().GetConfirmPressed() /*|| UnityEngine.N3DS.GamePad.GetButtonTrigger(N3dsButton.Start)*/)
            {
                if (Cutscene != null && Cutscene.State == CinemaDirector.Cutscene.CutsceneState.Playing)
                {
                    Cutscene.Skip();
                }
            }
        }

        /// <summary>
        /// If Cutscene is setup to play on trigger, watch for the trigger event.
        /// </summary>
        /// <param name="other">The other collider.</param>
        void OnTriggerEnter(Collider other)
        {
            if (StartMethod == StartMethod.OnTrigger && !hasTriggered && other.gameObject == TriggerObject)
            {
                hasTriggered = true;
                Cutscene.Play();
            }
        }

        /// <summary>
        /// If Cutscene is setup to play on trigger, watch for the trigger event.
        /// </summary>
        /// <param name="other">The other collider.</param>
        void OnTriggerEnter2D(Collider2D other)
        {
            if (StartMethod == StartMethod.OnTrigger && !hasTriggered && other.gameObject == TriggerObject)
            {
                hasTriggered = true;
                Cutscene.Play();
            }
        }


        /// <summary>
        /// If Cutscene is setup to play on button down and on trigger, watch for the trigger event.
        /// </summary>
        /// <param name="other">The other collider.</param>
        void OnTriggerStay(Collider other)
        {
            if (StartMethod == StartMethod.OnTriggerStayAndButtonDown && !hasTriggered && other.gameObject == TriggerObject && GameManager.Instance.GetPlayerInputManager().GetConfirmPressed())
            {
                hasTriggered = true;
                Cutscene.Play();
            }
        }

        void OnTriggerStay2D(Collider2D other)
        {
            if (StartMethod == StartMethod.OnTriggerStayAndButtonDown && !hasTriggered && other.gameObject == TriggerObject && GameManager.Instance.GetPlayerInputManager().GetConfirmPressed())
            {
                hasTriggered = true;
                Cutscene.Play();
            }
        }
    }

    public enum StartMethod
    {
        OnStart,
        OnTrigger,
        OnTriggerStayAndButtonDown,
        None
    }
}