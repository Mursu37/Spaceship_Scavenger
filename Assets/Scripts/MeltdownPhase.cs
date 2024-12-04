using Enviroment.MainTerminal;
using UnityEngine;
using UnityEngine.UIElements;

public class MeltdownPhase : MonoBehaviour
{
    [SerializeField] private GameObject heatMeter;
    [SerializeField] private GameObject coreObject;
    [SerializeField] private GameObject lightManager;
    [SerializeField] private GameObject objectiveMarker;
    [SerializeField] private ShipPowerOn shipPowerOn;
    private CoreSounds coreSounds;
    private AlarmSounds[] alarmSounds;
    private MeltdownMusic meltdownMusic;
    private LampSwitcherManager lampSwitcherManager;

    private AmbientMusic ambientMusic;

    private void OnEnable()
    {
        if (lightManager != null)
        {
            lampSwitcherManager = lightManager.GetComponent<LampSwitcherManager>();
        }

        if (!shipPowerOn.isPowerOn)
        {
            shipPowerOn.turnShipOnNoSound();
        }

        lampSwitcherManager.SetAlarmOn();

        coreObject.SetActive(true);

        if (heatMeter != null)
        {
            heatMeter.SetActive(true);
        }

        // Enable the CoreSounds, AlarmSounds and MeltdownMusic scripts when MeltdownPhase starts
        coreSounds = FindObjectOfType<CoreSounds>();
        if (coreSounds != null)
        {
            coreSounds.ActivateCoreSounds();
            // Debug.Log("CoreSounds enabled");
        }

        alarmSounds = FindObjectsOfType<AlarmSounds>();
        if (alarmSounds.Length > 0)
        {
            foreach (AlarmSounds sound in alarmSounds)
            {
                sound.ActivateAlarmSounds();
                // Debug.Log("Alarm sounds enabled");
            }
        }

        meltdownMusic = FindObjectOfType<MeltdownMusic>();
        if (meltdownMusic != null)
        {
            meltdownMusic.ActivateMeltdownMusic();
            // Debug.Log("Meltdown music enabled");
        }

        ambientMusic = FindObjectOfType<AmbientMusic>();
        if (ambientMusic != null)
        {
            ambientMusic.StopAmbientMusic();
        }
        
        if (objectiveMarker != null && !CheckpointManager.checkpointReached) objectiveMarker.SetActive(true);
    }
}
