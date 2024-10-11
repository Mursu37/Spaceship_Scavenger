using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreSounds : MonoBehaviour
{
    // private EnergyCore energyCore;
    // [SerializeField] GameObject energyCoreObject

//private void Start()
//{
//    energyCore = energyCoreObject.GetComponent<EnergyCore>();
//}

    private void Awake()
    {
        AudioManager.PlayAudio("CoreLoop", 1, 1, true);
        AudioManager.PlayAudio("CoreNoOverheat", 1, 1, true);
    }
}
