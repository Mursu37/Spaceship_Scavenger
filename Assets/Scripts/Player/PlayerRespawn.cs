using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    private bool canRespawn;
    private Vector3 spawnPosition;

    public static bool hasLoadedOnce = false;

    private void Awake()
    {
        if (!hasLoadedOnce)
        {
            PlayerPrefs.SetFloat("PlayerPosX", transform.position.x);
            PlayerPrefs.SetFloat("PlayerPosY", transform.position.y);
            PlayerPrefs.SetFloat("PlayerPosZ", transform.position.z);
            hasLoadedOnce = true;
        }
        canRespawn = true;

        spawnPosition = new Vector3(
            PlayerPrefs.GetFloat("PlayerPosX"),
            PlayerPrefs.GetFloat("PlayerPosY"),
            PlayerPrefs.GetFloat("PlayerPosZ"));

        Debug.Log("Spawned at: " + PlayerPrefs.GetFloat("PlayerPosX") + " " + PlayerPrefs.GetFloat("PlayerPosY") + " " + PlayerPrefs.GetFloat("PlayerPosZ"));
    }

    private void Update()
    {
        if (canRespawn)
        {
            transform.position = spawnPosition;
            canRespawn = false;
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
