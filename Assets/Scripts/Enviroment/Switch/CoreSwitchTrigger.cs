using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;

public class CoreSwitchTrigger : Switch
{
    protected override IEnumerator SwitchAction()
    {
        yield return new WaitForSeconds(0.5f);
        if (TryGetComponent<EventDispatcher>(out EventDispatcher foundDispatcher))
            foundDispatcher.TriggerEvent();

        GameManager.instance.UpdatePhase(Phase.Meltdown);
    }
}
