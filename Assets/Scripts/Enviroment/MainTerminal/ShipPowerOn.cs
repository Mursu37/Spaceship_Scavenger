using System;
using UnityEngine;

namespace Enviroment.MainTerminal
{
    public class ShipPowerOn : MonoBehaviour
    {
        private LampSwitcherManager lampSwitcherManager;

        [SerializeField] private GameObject lightManager;
        [SerializeField] private GameObject obstacles;
        [SerializeField] private Animator doorAnimator;
        [SerializeField] private EventDispatcher dispatcher;

        public bool isPowerOn = false;

        private void Awake()
        {
            obstacles.SetActive(false);
            lampSwitcherManager = lightManager.GetComponent<LampSwitcherManager>();
            if (dispatcher == null)
            {
                dispatcher = GetComponent<EventDispatcher>();
            }
        }

        public void turnShipOn()
        {
            lampSwitcherManager.ToggleLamps(true);
            obstacles.SetActive(true);
            dispatcher.TriggerEvent();
            //doorAnimator.Play("Closing");
            isPowerOn = true;
            AudioManager.PlayAudio("ShipPowerOn", 1, 1, false);
        }

        public void turnShipOnNoSound()
        {
            lampSwitcherManager.ToggleLamps(true);
            obstacles.SetActive(true);
            //doorAnimator.Play("Closing");
            isPowerOn = true;
        }
    }
}
