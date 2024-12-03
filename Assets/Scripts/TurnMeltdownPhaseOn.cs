using UnityEngine;

public class TurnMeltdownPhaseOn : MonoBehaviour
{
    private bool isTurnedOn = false;
    private MeltdownPhase meltdownPhase;

    private void Start()
    {
        meltdownPhase = GetComponent<MeltdownPhase>();
    }

    public void SetBool(bool _bool)
    {
        isTurnedOn = _bool;
    }

    private void Update()
    {
        if (!isTurnedOn)
        {
            TurnOn();
            //Invoke("TurnOn", 0.1f);
        }
    }

    private void TurnOn()
    {

        if (CheckpointManager.checkpointReached)
        {
            meltdownPhase.enabled = true;
        }

        isTurnedOn = true;
    }
}
