using UnityEngine;

public class MeltdownPhase : MonoBehaviour
{
    private EnergyCore energyCore;

    [SerializeField] private GameObject energyCoreObject;
    [SerializeField] private GameObject heatMeter;
    private CoreSounds coreSounds;

    private void Start()
    {
        if (gameObject != null)
        {
            energyCore = energyCoreObject.GetComponent<EnergyCore>();
            StartCoroutine(energyCore.HeatIncrease());
        }

        if (heatMeter != null)
        {
            heatMeter.SetActive(true);
        }

        // Enable the CoreSounds script when MeltdownPhase starts
        coreSounds = FindObjectOfType<CoreSounds>();
        if (coreSounds != null)
        {
            coreSounds.ActivateCoreSounds();
            Debug.Log("CoreSounds class enabled");
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}
