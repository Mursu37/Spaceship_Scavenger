using UnityEngine;

namespace Enviroment.MainTerminal
{
    public class Terminal : MonoBehaviour, IInteractable
    {
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void Interact()
        {
            Debug.Log("Ship on");
            GetComponent<ShipPowerOn>().turnShipOn();
        }
    }
}
