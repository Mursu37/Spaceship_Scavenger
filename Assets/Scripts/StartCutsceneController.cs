using UnityEngine;
using UnityEngine.Playables;

public class StartCutsceneController : MonoBehaviour
{
    [SerializeField] private PlayableDirector playableDirector1;
    [SerializeField] private PlayableDirector playableDirector2;
    [SerializeField] private PauseMenu pauseMenu;

    private void Start()
    {
        if (!CheckpointManager.checkpointReached && !CheckpointManager.engineRoomReached)
        {
            PauseGame.isPaused = true;
            pauseMenu.enabled = false;
        }

        if (!CheckpointManager.checkpointReached && !CheckpointManager.engineRoomReached)
        {
            playableDirector1.Play();
            playableDirector2.Play();
        }

        Invoke("ReleasePlayer", 15);
    }

    private void ReleasePlayer()
    {
        PauseGame.isPaused = false;
        pauseMenu.enabled = true;
    }
}
