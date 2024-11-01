using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbienceManager : MonoBehaviour
{
[SerializeField] private string derelictAmbience;
[SerializeField] private string meltdownAmbience;

private MeltdownPhase meltdownPhase;
private bool ambienceChanged = false;

    // Start is called before the first frame update
    private void Start()
    {
        meltdownPhase = FindObjectOfType<MeltdownPhase>();
        if (meltdownPhase == null)
        {
            Debug.LogWarning("MeldownPhase not found");
        }

        AudioManager.PlayAudio(derelictAmbience, 1, 1, true);
    }

    // Update is called once per frame
    private void Update()
    {
        if (meltdownPhase != null && meltdownPhase.enabled && !ambienceChanged)
        {
            ChangeAmbience();
            ambienceChanged = true;
        }
    }

    private void ChangeAmbience()
    {
        AudioManager.StopAudio(derelictAmbience);
        AudioManager.PlayAudio(meltdownAmbience, 1, 1, true);
    }

}
