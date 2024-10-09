using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    private bool canRespawn;
    private Vector3 spawnPosition;

    [SerializeField] private GameObject spawnPoint;

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
        if (spawnPoint != null)
        {
            PlayerPrefs.SetFloat("PlayerPosX", spawnPoint.transform.position.x);
            PlayerPrefs.SetFloat("PlayerPosY", spawnPoint.transform.position.y);
            PlayerPrefs.SetFloat("PlayerPosZ", spawnPoint.transform.position.z);
        }
        else
        {
            PlayerPrefs.SetFloat("PlayerPosX", 0);
            PlayerPrefs.SetFloat("PlayerPosY", 0);
            PlayerPrefs.SetFloat("PlayerPosZ", 0);
        }
    }
}
