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
                        break;
                    }
                }

                energyCore.heatAmount = CheckpointManager.coreHealth;
            }

            coreRespawned = true;
        }
    }
}
