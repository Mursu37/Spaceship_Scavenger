using UnityEngine;

public class MeltdownPhase : MonoBehaviour
{
    [SerializeField] private GameObject heatMeter;
    private CoreSounds coreSounds;
    private MeltdownMusic meltdownMusic;

    private void Start()
    {
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
    }
}
