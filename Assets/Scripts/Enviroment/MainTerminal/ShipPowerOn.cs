using System;
using TMPro;
using UnityEngine;

namespace Enviroment.MainTerminal
{
    public class ShipPowerOn : MonoBehaviour
    {
        private LampSwitcherManager lampSwitcherManager;

        [SerializeField] private GameObject lightManager;
        [SerializeField] private EventDispatcher dispatcher;
        [SerializeField] private TMP_Text powerText;

        public bool isPowerOn = false;

        private void Awake()
        {
            lampSwitcherManager = lightManager.GetComponent<LampSwitcherManager>();
            if (dispatcher == null)
            {
                dispatcher = GetComponent<EventDispatcher>();
            }
        }

        public void turnShipOn()
        {
            lampSwitcherManager.ToggleLamps(true);
            dispatcher.TriggerEvent();
            isPowerOn = true;
            AudioManager.PlayAudio("ShipPowerOn", 1, 1, false);
            powerText.text = "MAIN POWER ON";
            powerText.color = Color.green;
        }

        public void turnShipOnNoSound()
        {
            lampSwitcherManager.ToggleLamps(true);
            dispatcher.TriggerEvent();
            isPowerOn = true;
        }
    }
}
