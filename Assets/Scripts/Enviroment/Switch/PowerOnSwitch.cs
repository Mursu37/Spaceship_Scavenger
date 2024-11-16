using Enviroment.MainTerminal;
using System.Collections;
using UnityEngine;

public class PowerOnSwitch : Switch
{
    private ShipPowerOn shipPowerOn;

    protected override IEnumerator SwitchAction()
    {
        yield return new WaitForSeconds(1f);
        shipPowerOn = GetComponent<ShipPowerOn>();
        shipPowerOn.turnShipOn();
    }
}
