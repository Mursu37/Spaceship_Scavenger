using UnityEngine;

public class MeltdownPhase : MonoBehaviour
{
    private EnergyCore energyCore;
    private bool canSpawn = false;

    [SerializeField] GameObject gameStates;
    [SerializeField] GameObject heathMeter;

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

        if (heathMeter != null)
        {
            heathMeter.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
