using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    private bool playerRespawned = false;

    private void Update()
    {
        if (!playerRespawned)
        {
            if (CheckpointManager.checkpointReached || CheckpointManager.engineRoomReached)
            {
                transform.position = CheckpointManager.lastCheckpointPosition;
            }

            playerRespawned = true;
        }
    }
}
