using UnityEngine;

public class MeltdownPhase : MonoBehaviour
{
    private EnergyCore energyCore;
    private bool canSpawn = false;

    [SerializeField] private GameObject gameStates;
    [SerializeField] private GameObject heatMeter;
    [SerializeField] private GameObject meltdownObjects;

    private void Awake()
    {
        if (enabled)
        {
            enabled = false;
        }
    }

    private void Start()
    {
        canSpawn = true;

        if (gameObject != null)
        {
            energyCore = gameStates.GetComponent<EnergyCore>();
            StartCoroutine(energyCore.HeathIncrease());
        }

        if (heatMeter != null)
        {
            heatMeter.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (canSpawn)
        {
            meltdownObjects.SetActive(true);
            canSpawn = false;
        }
    }
}
