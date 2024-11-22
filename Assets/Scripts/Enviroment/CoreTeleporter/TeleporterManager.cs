using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporterManager : MonoBehaviour
{
    public Dictionary<int, GameObject> teleportExitDictionary;

    private void Start()
    {
        teleportExitDictionary = new Dictionary<int, GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
