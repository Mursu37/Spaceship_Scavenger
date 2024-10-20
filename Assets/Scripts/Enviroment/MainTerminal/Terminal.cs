using UnityEngine;

namespace Enviroment.MainTerminal
{
    public class Terminal : MonoBehaviour, IInteractable
    {
        public void Interact()
        {
            Debug.Log("Ship on");
            GetComponent<ShipPowerOn>().turnShipOn();
        }
    }
}
