using UnityEngine;

public class TurnMeltdownPhaseOn : MonoBehaviour
{
    private bool isTurnedOn = false;
    private MeltdownPhase meltdownPhase;

    private void Start()
    {
        meltdownPhase = GetComponent<MeltdownPhase>();
    }

    private void Update()
    {
        if (!isTurnedOn)
        {
            if (CheckpointManager.checkpointReached)
            {
                meltdownPhase.enabled = true;
            }

            isTurnedOn = true;
        }
    }
}
