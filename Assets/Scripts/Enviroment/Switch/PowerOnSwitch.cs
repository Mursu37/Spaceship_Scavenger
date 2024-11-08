using Enviroment.MainTerminal;
using System.Collections;

public class PowerOnSwitch : Switch
{
    private ShipPowerOn shipPowerOn;

    protected override IEnumerator SwitchAction()
    {
        shipPowerOn = GetComponent<ShipPowerOn>();
        shipPowerOn.turnShipOn();
        yield return null;
    }
}
