using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasLeakManager : MonoBehaviour
{
    private ParticleSystem gasParticle;
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
            Debug.Log("Checking for dispensers.");
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
                    gasParticle = leak.transform.GetChild(0).GetComponent<ParticleSystem>();
                    if (gasTrigger != null && gasParticle != null)
                    {
                        if (currentState == LeakState.Disabled)
                        {
                            gasParticle.Stop();
                            gasTrigger.enabled = false;
                        }
                        else if (currentState == LeakState.Enabled)
                        {
                            gasParticle.Play();
                            gasTrigger.enabled = true;
                        }
                    }
                }
            }
        }
    }
}
