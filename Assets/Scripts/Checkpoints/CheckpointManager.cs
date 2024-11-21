using System.Collections.Generic;
using UnityEngine;

public static class CheckpointManager
{
    // Save checkpoint data
    public static bool checkpointReached = false; // Checks if player has reached a checkpoint at any time during the session
    public static Vector3 lastCheckpointPosition = Vector3.zero; // Saves the position of the last checkpoint
    
    public static int lastTeleportId; // Saves the ID of the most recently used teleporter exit which can be used to determine where the core will teleport

    public static List<int> doorsOpened = new List<int>(); // Keeps track of which doors are opened

    public static int currentActiveMarkerId; // Saves the current active marker ID

    public static float coreHealth; // Assigns health value for core each respawn

    // Saves the most recent checkpoint
    public static void SaveCheckpoint(Vector3 checkpointPosition)
    {
        checkpointReached = true;
        lastCheckpointPosition = checkpointPosition;
    }

    // Reset checkpoint data
    public static void ResetCheckpoints()
    {
        lastCheckpointPosition = Vector3.zero;
        checkpointReached = false;
    }
}
