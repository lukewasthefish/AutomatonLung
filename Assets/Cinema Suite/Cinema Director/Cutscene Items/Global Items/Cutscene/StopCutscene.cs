// Cinema Suite
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CinemaDirector
{
    /// <summary>
    /// Stop a given Cutscene from playing.
    /// </summary>
    [CutsceneItem("Cutscene", "Stop Cutscene", CutsceneItemGenre.GlobalItem)]
    public class StopCutscene : CinemaGlobalEvent
    {
        // The cutscene to be stopped.
        public Cutscene cutscene;

        /// <summary>
        /// Trigger this event and stop the given Cutscene from playing.
        /// </summary>
        public override void Trigger()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.destinationScene = cutscene.destinationScene;

                GameManager.Instance.LoadAfterDelay(0.5f, cutscene.destinationScene);
            }

            if (cutscene != null)
            {
                cutscene.Stop();
            }
        }
    }
}