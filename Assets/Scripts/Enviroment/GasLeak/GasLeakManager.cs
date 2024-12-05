using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasLeakManager : MonoBehaviour
{
    private ParticleSystem[] gasParticles;
    private GasTrigger gasTrigger;
    private LeakState currentState;
    private LeakState previousState;

    [SerializeField] private GameObject gasLeaks;
    [SerializeField] private DispenserController[] dispensers;

    private enum LeakState
    {
        Enabled,
        Disabled
    }

    private void Start()
    {
        currentState = LeakState.Enabled;
        previousState = currentState;
    }

    private void Update()
    {
        if (dispensers != null)
        {
            bool canActivate = true;

            foreach (var dispenser in dispensers)
            {
                if (!dispenser.isOpen)
                {
                    currentState = LeakState.Enabled;
                    canActivate = false;
                }
            }

            if (canActivate)
            {
                currentState = LeakState.Disabled;
                UpdateGasLeaks();
            }

            if (currentState != previousState)
            {
                previousState = currentState;
                UpdateGasLeaks();
            }
        }
    }

    private void UpdateGasLeaks()
    {
        if (gasLeaks != null)
        {
            for (int i = 0; i < gasLeaks.transform.childCount; i++)
            {
                GameObject leak = gasLeaks.transform.GetChild(i).gameObject;
                if (leak != null)
                {
                    gasTrigger = leak.GetComponent<GasTrigger>();
                    gasParticles = leak.transform.GetComponentsInChildren<ParticleSystem>();
                    
                    for (int j = 0; j < gasParticles.Length; j++)
                    {
                        if (gasTrigger != null && gasParticles[j] != null)
                        {
                            if (currentState == LeakState.Disabled)
                            {
                                gasParticles[j].Stop();
                                gasTrigger.enabled = false;
                            }
                            else if (currentState == LeakState.Enabled)
                            {
                                gasParticles[j].Play();
                                gasTrigger.enabled = true;
                            }
                        }
                    }

                }
            }
        }
    }
}
