using System.Collections;
using UnityEngine;

public class EngineRoomSwitch : Switch
{
    public bool isEngineCompuerOn = false;

    protected override IEnumerator SwitchAction()
    {
        while (true)
        {
            yield return new WaitForSeconds(0f);
            isEngineCompuerOn = true;
        }
    }
}
