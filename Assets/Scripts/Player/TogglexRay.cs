using System.Collections.Generic;
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

    [SerializeField] private GameObject lowPolyPrefab;
    private List<GameObject> lowPolyObjects = new List<GameObject>();

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
            LowPolyLayer(LayerMask.GetMask("Dynamic"));
        }
    }

    private void LowPolyLayer(LayerMask mask)
    {
        if (lowPolyPrefab == null) return;
        Collider[] colliders = FindObjectsByType<Collider>(FindObjectsSortMode.None);
        lowPolyObjects.Clear();

        foreach (var collider in colliders)
        {
            if ((mask & (1 << collider.gameObject.layer)) != 0)
            {
                var lowPolyObject = Instantiate(lowPolyPrefab, collider.transform);
                lowPolyObject.transform.localScale = collider.bounds.size;
                lowPolyObject.transform.localPosition =
                    collider.transform.InverseTransformPoint(collider.bounds.center);
                lowPolyObjects.Add(lowPolyObject);
            }
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
        foreach (var obj in lowPolyObjects)
        {
            Destroy(obj);
        }
        lowPolyObjects.Clear();
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
