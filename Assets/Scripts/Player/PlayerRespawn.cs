using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    private bool playerRespawned = false;

    private void Update()
    {
        if (!playerRespawned)
        {
            if (CheckpointManager.checkpointReached)
            {
                transform.position = CheckpointManager.lastCheckpointPosition;
                Debug.Log($"Player respawned at {CheckpointManager.lastCheckpointPosition}");
            }
            else
            {
                Debug.Log("No checkpoint reached. Respawning at default position.");
            }

            playerRespawned = true;
        }
    }
}
