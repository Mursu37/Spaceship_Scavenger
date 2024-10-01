using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    private bool canRespawn;
    private Vector3 spawnPosition;

    private void Awake()
    {
        canRespawn = true;
        spawnPosition = new Vector3(
            PlayerPrefs.GetFloat("PlayerPosX"),
            PlayerPrefs.GetFloat("PlayerPosY"),
            PlayerPrefs.GetFloat("PlayerPosZ"));

        Debug.Log("Spawned at: " + PlayerPrefs.GetFloat("PlayerPosX") + " " + PlayerPrefs.GetFloat("PlayerPosY") + " " + PlayerPrefs.GetFloat("PlayerPosZ"));
    }

    private void FixedUpdate()
    {
        if (canRespawn)
        {
            transform.position = spawnPosition;
            canRespawn = false;
        }
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetFloat("PlayerPosX", 17.5f);
        PlayerPrefs.SetFloat("PlayerPosY", -0.884f);
        PlayerPrefs.SetFloat("PlayerPosZ", 14.3f);
    }
}
