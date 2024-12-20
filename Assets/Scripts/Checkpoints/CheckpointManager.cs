using System.Collections.Generic;
using UnityEngine;

public static class CheckpointManager
{
    // Save checkpoint data
    public static bool checkpointReached = false; // Checks if player has reached a checkpoint at any time during the session

    public static bool engineRoomReached = false; // Checks if player has reached a engine room

    public static Vector3 lastCheckpointPosition = Vector3.zero; // Saves the position of the last checkpoint
    
    public static int lastTeleportId; // Saves the ID of the most recently used teleporter exit which can be used to determine where the core will teleport

    public static int currentActiveMarkerId; // Saves the current active marker ID

    public static List<int> doorsOpened = new List<int>(); // Keeps track of which doors are opened

    public static List<int> teleportersUsed = new List<int>(); // Keeps track of which teleports were used

    public static List<int> switchesTurnedOn = new List<int>(); // Tracks which switches were turned on

    public static List<string> tutorialsShowed = new List<string>(); // Tracks which tutorials have been showed

    public static bool captainsCredentialsGained = false; // Tracks if player has visited Captain's quarters and downloaded keys

    public static float coreHealth; // Assigns health value for core each respawn

    // Saves the most recent checkpoint
    public static void SaveCheckpoint(Vector3 checkpointPosition)
    {
        checkpointReached = true;
        lastCheckpointPosition = checkpointPosition;
    }

    public static void EngineRoomCheckpoint(Vector3 checkpointPosition)
    {
        engineRoomReached = true;
        lastCheckpointPosition = checkpointPosition;
    }

    // Reset checkpoint data
    public static void ResetCheckpoints()
    {
        checkpointReached = false;
        engineRoomReached = false;
        captainsCredentialsGained = false;
        lastCheckpointPosition = Vector3.zero;
        lastTeleportId = 0;
        currentActiveMarkerId = 0;
        doorsOpened.Clear();
        switchesTurnedOn.Clear();
        coreHealth = 0;
        tutorialsShowed.Clear();
        teleportersUsed.Clear();
    }
}
