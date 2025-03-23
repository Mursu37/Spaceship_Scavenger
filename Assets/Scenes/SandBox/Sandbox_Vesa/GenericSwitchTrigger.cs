using CLI.FSM;
using Enviroment.MainTerminal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericSwitchTrigger : Switch
{
    [SerializeField]
    private EventDispatcher dispatcher;

    protected override IEnumerator SwitchAction()
    {
        yield return new WaitForSeconds(0.5f);
        dispatcher.TriggerEvent();
    }
}
