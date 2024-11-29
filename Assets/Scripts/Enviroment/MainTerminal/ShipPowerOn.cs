using System;
using UnityEngine;

namespace Enviroment.MainTerminal
{
    public class ShipPowerOn : MonoBehaviour
    {
        private LampSwitcherManager lampSwitcherManager;

        [SerializeField] private GameObject lightManager;
        [SerializeField] private Animator doorAnimator;
        [SerializeField] private EventDispatcher dispatcher;

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
            //doorAnimator.Play("Closing");
            isPowerOn = true;
            AudioManager.PlayAudio("ShipPowerOn", 1, 1, false);
        }

        public void turnShipOnNoSound()
        {
            lampSwitcherManager.ToggleLamps(true);
            //doorAnimator.Play("Closing");
            dispatcher.TriggerEvent();
            isPowerOn = true;
        }
    }
}
