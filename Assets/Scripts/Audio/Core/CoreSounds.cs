using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreSounds : MonoBehaviour
{

    [SerializeField] private string baseLoop;
    [SerializeField] private string noOverheatLoop;
    [SerializeField] private string slightOverheatLoop;
    [SerializeField] private string heavyOverheatLoop;

    [SerializeField] private string[] damageSounds; 

    private EnergyCore core;
    private float previousHeatPercent;
    private bool isActive = false;

    private void Start()
    {
        core = GetComponentInParent<EnergyCore>();
        //this.enabled = false;
  
    }


    private void Update()
    {
        if (!isActive || core == null) return;
        

        float heatPercent = core.heatAmount / core.maxHeat;

        // Check for heat level thresholds and switch layer sounds accordingly
        if (heatPercent >= 0.66f && previousHeatPercent < 0.66f)
        {
            ChangeLayerLoop(heavyOverheatLoop);
        }
        else if (heatPercent >= 0.33f && previousHeatPercent < 0.33f)
        {
            ChangeLayerLoop(slightOverheatLoop);
        }
        else if (heatPercent < 0.33f && previousHeatPercent >= 0.33f)
        {
            ChangeLayerLoop(noOverheatLoop);
        }

        previousHeatPercent = heatPercent;
    }

    private void ChangeLayerLoop(string newLayerName)
    {

        // Stop all overheat layer sounds before starting the new layer
        AudioManager.StopAudio(noOverheatLoop);
        AudioManager.StopAudio(slightOverheatLoop);
        AudioManager.StopAudio(heavyOverheatLoop);

        // Start the new layer loop
        AudioManager.PlayAudio(newLayerName, 1, 1, true);
    }

    public void ActivateCoreSounds()
    {
        this.enabled = true; 
        isActive = true;
        AudioManager.PlayAudio(baseLoop, 1, 1, true);
        AudioManager.PlayAudio(noOverheatLoop, 1, 1, true);
        Debug.Log("Playing containment core sounds");
    }

    public void PlayRandomDamageSound()
    {
        if (damageSounds.Length > 0)
        {
            int randomIndex = Random.Range(0, damageSounds.Length);
            string randomSoundName = damageSounds[randomIndex];
            AudioManager.PlayAudio(randomSoundName, 1, 1, false);  
        }
    }

    public void PlayExplosionSounds(GameObject parentObject)
    {
        AudioManager.PlayFollowedAudio("CoreExplosionCharge", parentObject, 0.4f, 1, false);
        StartCoroutine(PlayBigBoom());
    }

    private IEnumerator PlayBigBoom()
    {
        float chargeDuration = 5.1f;
        yield return new WaitForSeconds(chargeDuration);
        AudioManager.PlayAudio("CoreExplosionBoom", 1, 1, false);

        GameObject shockwaveObject = new  GameObject("ShockwaveSoundObject");
        shockwaveObject.transform.position = transform.position;
        AudioManager.PlayFollowedAudio("CoreExplosionShockwave", shockwaveObject, 1, 1, true);

        Vector3 playerPosition = GameObject.FindWithTag("Player").transform.position;
        float moveSpeed = 11f; //slightly slower than the particle effect to avoid panning issues
        float timeToReachPlayer = Vector3.Distance(shockwaveObject.transform.position, playerPosition) / moveSpeed;

        float elapsedTime = 0f;
        while (elapsedTime < timeToReachPlayer)
        {
            shockwaveObject.transform.position = Vector3.MoveTowards(shockwaveObject.transform.position, playerPosition, moveSpeed * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        shockwaveObject.transform.position = playerPosition;
    }

}
