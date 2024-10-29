using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreSounds : MonoBehaviour
{

    [SerializeField] private string baseLoop;
    [SerializeField] private string noOverheatLoop;
    [SerializeField] private string slightOverheatLoop;
    [SerializeField] private string heavyOverheatLoop;

    [SerializeField] private string[] damageSounds; 

    

    private EnergyCore core;
    private float previousHeatPercent;
    private bool isActive = false;

    private void Start()
    {
        core = GetComponentInParent<EnergyCore>();
        this.enabled = false;
  
    }


    private void Update()
    {
        if (!isActive || core == null) return;
        

        float heatPercent = core.heatAmount / core.maxHeat;

        // Check for heat level thresholds and switch layer sounds accordingly
        if (heatPercent >= 0.66f && previousHeatPercent < 0.66f)
        {
            ChangeLayerLoop(heavyOverheatLoop);
        }
        else if (heatPercent >= 0.33f && previousHeatPercent < 0.33f)
        {
            ChangeLayerLoop(slightOverheatLoop);
        }
        else if (heatPercent < 0.33f && previousHeatPercent >= 0.33f)
        {
            ChangeLayerLoop(noOverheatLoop);
        }

        previousHeatPercent = heatPercent;
    }

    private void ChangeLayerLoop(string newLayerName)
    {

        // Stop all overheat layer sounds before starting the new layer
        AudioManager.StopAudio(noOverheatLoop);
        AudioManager.StopAudio(slightOverheatLoop);
        AudioManager.StopAudio(heavyOverheatLoop);

        // Start the new layer loop
        AudioManager.PlayAudio(newLayerName, 1, 1, true);
    }

    public void ActivateCoreSounds()
    {
        this.enabled = true; 
        isActive = true;
        AudioManager.PlayAudio(baseLoop, 1, 1, true);
        AudioManager.PlayAudio(noOverheatLoop, 1, 1, true);
        Debug.Log("Playing containment core sounds");
    }

    public void PlayRandomDamageSound()
    {
        if (damageSounds.Length > 0)
        {
            int randomIndex = Random.Range(0, damageSounds.Length);
            string randomSoundName = damageSounds[randomIndex];
            AudioManager.PlayAudio(randomSoundName, 1, 1, false);  
        }
    }


}
