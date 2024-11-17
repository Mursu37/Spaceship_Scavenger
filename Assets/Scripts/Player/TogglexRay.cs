using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class TogglexRay : MonoBehaviour
{
    [SerializeField] private CustomPassVolume customPassVolume;
    private bool xRayActive;
    
    [SerializeField] private float expansionRate;
    [SerializeField] private float maxExpansionDistance;
    [SerializeField] private float timeBeforeExpiration;
    
    private float timer;
    private float range;

    private void Start()
    {
        xRayActive = false;
        timer = 0;
        range = 0;
        customPassVolume.enabled = xRayActive;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Scan") && !PauseGame.isPaused)
        {
            if (xRayActive)
            {
                XRayEnded();
                return;
            }
            xRayActive = true;
            customPassVolume.enabled = true;
            AudioManager.PlayAudio("XrayOn", 1, 1, false);
        }
    }

    public void ActivateXraypulse()
    {
        xRayActive = true;
        customPassVolume.enabled = true;
    }

    private void FixedUpdate()
    {
        if (!xRayActive) return;
        timer += Time.fixedDeltaTime;
        if (timer > timeBeforeExpiration)
        {
            XRayEnded();
            return;
        }
        if (range >= maxExpansionDistance) return;

        range += Time.fixedDeltaTime * expansionRate;
        if (range > maxExpansionDistance) range = maxExpansionDistance;
    }

    public void XRayEnded()
    {
        xRayActive = false;
        timer = 0;
        range = 0;
        customPassVolume.enabled = xRayActive;
    }

    public float GetRange()
    {
        return range;
    }
}
