using UnityEngine;

public class CoreRespawn : MonoBehaviour
{
    private bool coreRespawned = false;
    private EnergyCore energyCore;

    private void Start()
    {
        energyCore = GetComponent<EnergyCore>();
    }

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

                energyCore.heatAmount = CheckpointManager.coreHealth;
            }
            else
            {
                Debug.Log("Respawning Core at default position.");
            }

            coreRespawned = true;
        }
    }
}
