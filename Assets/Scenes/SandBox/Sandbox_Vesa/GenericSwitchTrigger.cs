using CLI.FSM;
using Enviroment.MainTerminal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericSwitchTrigger : Switch
{
    private Animator switchAnimator;
    [SerializeField]
    private EventDispatcher dispatcher;

    // Start is called before the first frame update
    void Start()
    {
        switchAnimator = gameObject.GetComponentInChildren<Animator>();
    }

    protected override IEnumerator SwitchAction()
    {
        yield return new WaitForSeconds(0.5f);
        dispatcher.TriggerEvent();
        //ActivateSwitch();
    }

    //public override void Interact()
    //{
    //    if (!turnedOn)
    //    {
    //        StartCoroutine(SwitchAction());
    //    }
    //}

    // Why does this exist? It's just repeating the same stuff that the Switch class already does.
    private void ActivateSwitch()
    {
        if (switchAnimator.enabled == false)
        {
            return;
        }

        turnedOn = true;

        int layerIndex = 0;
        string stateName = "TurnOn";
        int stateHash = Animator.StringToHash(stateName);

        if (switchAnimator.HasState(layerIndex, stateHash))
        {
            switchAnimator.Play("TurnOn", layerIndex);
            dispatcher.TriggerEvent();

        }
    }

}
