using UI.HUD;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public void SaveCheckpointState(int teleportId)
    {
        CheckpointManager.SaveCheckpoint(transform.position);

        DoorController[] doorControllers = FindObjectsOfType<DoorController>();
        foreach(DoorController controller in doorControllers)
        {
            if (controller.doorOpened)
            {
                if (!CheckpointManager.doorsOpened.Contains(controller.id))
                {
                    CheckpointManager.doorsOpened.Add(controller.id);
                }
            }
        }

        Switch[] switches = FindObjectsOfType<Switch>();
        foreach(Switch sw in switches)
        {
            if (sw.turnedOn)
            {
                if (!CheckpointManager.switchesTurnedOn.Contains(sw.id))
                {
                    CheckpointManager.switchesTurnedOn.Add(sw.id);
                }
            }
        }

        if (teleportId == 1)
        {
            CheckpointManager.currentActiveMarkerId = 5;
            CheckpointManager.coreHealth = 5f;
        }
        else if (teleportId == 2)
        {
            CheckpointManager.currentActiveMarkerId = 7;
            CheckpointManager.coreHealth = 20f;
        }
    }
}
