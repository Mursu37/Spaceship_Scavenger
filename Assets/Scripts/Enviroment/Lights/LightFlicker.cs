using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private Light lightSource;

    [Header("Randomize Flickering")]
    [Tooltip("The maximum delay between flickers.")]
    [SerializeField] private float maxDelayBetweenFlickers = 1;
    [Tooltip("The maximum duration for flicker.")]
    [SerializeField] private float maxFlickerDuration = 0.2f;

    private float timer;
    private float interval;

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > interval)
        {
            ToggleLight();
        }
    }

    private void ToggleLight()
    {
        lightSource.enabled = !lightSource.enabled;
        if (lightSource.enabled)
        {
            interval = Random.Range(0, maxDelayBetweenFlickers);
        }
        else
        {
            interval = Random.Range(0, maxFlickerDuration);
        }

        timer = 0;
    }
}
