using UnityEngine;
using UnityEngine.Playables;

public class StartCutsceneController : MonoBehaviour
{
    [SerializeField] private PlayableDirector playableDirector1;
    [SerializeField] private PlayableDirector playableDirector2;

    private void Start()
    {
        if (!CheckpointManager.checkpointReached)
        {
            playableDirector1.Play();
            playableDirector2.Play();
        }
    }
}
