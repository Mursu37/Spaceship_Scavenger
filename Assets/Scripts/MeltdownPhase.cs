using UnityEngine;

public class MeltdownPhase : MonoBehaviour
{
    private EnergyCore energyCore;

    [SerializeField] private GameObject energyCoreObject;
    [SerializeField] private GameObject heatMeter;

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
    }

    // Update is called once per frame
    void Update()
    {
    }
}
