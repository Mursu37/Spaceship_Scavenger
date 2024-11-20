using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointSave : MonoBehaviour
{
    [SerializeField] private Transform core;

    public void Save()
    {
        CheckPointLoad.hasLoadedOnce = true;

        PlayerPrefs.SetFloat("PlayerPosX", transform.position.x);
        PlayerPrefs.SetFloat("PlayerPosY", transform.position.y);
        PlayerPrefs.SetFloat("PlayerPosZ", transform.position.z);

        PlayerPrefs.SetFloat("CorePosX", core.position.x);
        PlayerPrefs.SetFloat("CorePosY", core.position.y);
        PlayerPrefs.SetFloat("CorePosZ", core.position.z);

        PlayerPrefs.SetInt("IsMeltdownPhaseOn", 1);

        for (int i = 0; i < DoorStateController.doorPositions.Count; i++)
        {
            PlayerPrefs.SetFloat("Door_"+ i +"_PosX", DoorStateController.doorPositions[i].x);
            PlayerPrefs.SetFloat("Door_"+ i +"_PosY", DoorStateController.doorPositions[i].y);
            PlayerPrefs.SetFloat("Door_"+ i +"_PosZ", DoorStateController.doorPositions[i].z);

            Debug.Log("New door position for door " + i + ": " + PlayerPrefs.GetFloat("Door_" + i + "_PosX") + " " + PlayerPrefs.GetFloat("Door_" + i + "_PosY") + " " + PlayerPrefs.GetFloat("Door_" + i + "_Posx"));
        }
    }
}
