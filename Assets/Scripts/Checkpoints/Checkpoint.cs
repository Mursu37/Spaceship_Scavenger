using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public void SaveCheckpointPosition()
    {
        CheckpointManager.SaveCheckpoint(transform.position);
    }
}
