using UnityEngine;

public class MeltdownPhase : MonoBehaviour
{
    [SerializeField] private GameObject heatMeter;
    [SerializeField] private GameObject coreObject;
    [SerializeField] private GameObject lightManager;
    [SerializeField] private GameObject objectiveMarker;
    private CoreSounds coreSounds;
    private MeltdownMusic meltdownMusic;
    private LampSwitcherManager lampSwitcherManager;

    private AmbientMusic ambientMusic;

    private void OnEnable()
    {
        if (lightManager != null)
        {
            lampSwitcherManager = lightManager.GetComponent<LampSwitcherManager>();
        }

        lampSwitcherManager.SetAlarmOn();

        coreObject.SetActive(true);

        if (heatMeter != null)
        {
            heatMeter.SetActive(true);
        }

        // Enable the CoreSounds and MeltdownMusic scripts when MeltdownPhase starts
        coreSounds = FindObjectOfType<CoreSounds>();
        if (coreSounds != null)
        {
            coreSounds.ActivateCoreSounds();
            Debug.Log("CoreSounds enabled");
        }

        meltdownMusic = FindObjectOfType<MeltdownMusic>();
        if (meltdownMusic != null)
        {
            meltdownMusic.ActivateMeltdownMusic();
            Debug.Log("Meltdown music enabled");
        }

        ambientMusic = FindObjectOfType<AmbientMusic>();
        if (ambientMusic != null)
        {
            ambientMusic.StopAmbientMusic();
        }
        
        if (objectiveMarker != null) objectiveMarker.SetActive(true);
    }
}
