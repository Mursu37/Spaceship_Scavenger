using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorStateController : MonoBehaviour
{
    public static List<Vector3> doorPositions = new List<Vector3>();

    private void Start()
    {
        GameObject[] doors = GameObject.FindGameObjectsWithTag("Door");

        foreach (GameObject door in doors)
        {
            doorPositions.Add(door.transform.position);
            Debug.Log(door.name);
        }
    }
}
