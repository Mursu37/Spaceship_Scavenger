using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointLoad : MonoBehaviour
{
    private bool hasLoaded;

    [SerializeField] private Transform player;
    [SerializeField] private Transform core;
    [SerializeField] private MeltdownPhase meltdownPhase;

    public static bool hasLoadedOnce = false;

    private void Awake()
    {
        Debug.Log("Spawned at: " + PlayerPrefs.GetFloat("PlayerPosX") + " " + PlayerPrefs.GetFloat("PlayerPosY") + " " + PlayerPrefs.GetFloat("PlayerPosZ"));
    }

    private void Update()
    {
        if (!hasLoaded && hasLoadedOnce)
        {
            player.position = new Vector3(PlayerPrefs.GetFloat("PlayerPosX"),
                                          PlayerPrefs.GetFloat("PlayerPosY"),
                                          PlayerPrefs.GetFloat("PlayerPosZ"));

            core.position = new Vector3(PlayerPrefs.GetFloat("CorePosX"),
                                        PlayerPrefs.GetFloat("CorePosY"),
                                        PlayerPrefs.GetFloat("CorePosZ"));

            if (PlayerPrefs.GetInt("IsMeltdownPhaseOn") == 1)
            {
                meltdownPhase.enabled = true;
            }

            for (int i = 0; i < DoorStateController.doorPositions.Count; i++)
            {
                float posX = PlayerPrefs.GetFloat("Door_" + i + "_PosX");
                float posY = PlayerPrefs.GetFloat("Door_" + i + "_PosY");
                float posZ = PlayerPrefs.GetFloat("Door_" + i + "_PosZ");

                DoorStateController.doorPositions[i] = new Vector3(posX, posY, posZ);
            }

            hasLoaded = true;
        }
    }

    [RuntimeInitializeOnLoadMethod]
    private static void OnApplicationStart()
    {
        Application.quitting += ResetHasLoadedOnce;
    }

    private static void ResetHasLoadedOnce()
    {
        hasLoadedOnce = false;
    }
}
