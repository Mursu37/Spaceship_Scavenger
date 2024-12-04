using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineRoomCheckpoint : MonoBehaviour
{
    [SerializeField] private Transform checkpoint;

    private void OnTriggerEnter(Collider other)
    {
        if (!CheckpointManager.engineRoomReached && other.CompareTag("Player"))
        {
            CheckpointManager.EngineRoomCheckpoint(checkpoint.position);

            DoorController[] doorControllers = FindObjectsOfType<DoorController>();
            foreach (DoorController controller in doorControllers)
            {
                if (controller.doorOpened)
                {
                    if (!CheckpointManager.doorsOpened.Contains(controller.id))
                    {
                        CheckpointManager.doorsOpened.Add(controller.id);
                    }
                }
            }

            CheckpointManager.currentActiveMarkerId = 4;
        }
        
    }
}
