using UnityEngine;

public class CoreRespawn : MonoBehaviour
{
    private bool coreRespawned = false;

    private void Update()
    {
        if (!coreRespawned)
        {
            if (CheckpointManager.checkpointReached)
            {
                CoreTeleporterExit[] teleporterExits = FindObjectsOfType<CoreTeleporterExit>();
                foreach (CoreTeleporterExit teleporterExit in teleporterExits)
                {
                    if (teleporterExit.id == CheckpointManager.lastTeleportId)
                    {
                        teleporterExit.currentState = CoreTeleporterExit.TeleporterState.Idle;
                        teleporterExit.StartTeleportation();
                        Debug.Log($"ID {teleporterExit.id} matches with the last teleport ID.");
                        break;
                    }
                }

                Debug.Log("Core at: " + transform.position);
            }
            else
            {
                Debug.Log("Respawning Core at default position.");
            }

            coreRespawned = true;
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            if (CheckpointManager.checkpointReached)
            {
                CoreTeleporterExit[] teleporterExits = FindObjectsOfType<CoreTeleporterExit>();
                foreach (CoreTeleporterExit teleporterExit in teleporterExits)
                {
                    if (teleporterExit.id == CheckpointManager.lastTeleportId)
                    {
                        teleporterExit.currentState = CoreTeleporterExit.TeleporterState.Idle;
                        teleporterExit.StartTeleportation();
                        Debug.Log("ID found!");
                        break;
                    }
                }

                Debug.Log("Core at: " + transform.position);
            }
            else
            {
                Debug.Log("Respawning Core at default position.");
            }
        }
    }
}
