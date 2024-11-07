using System;
using UnityEngine;

namespace Enviroment.MainTerminal
{
    public class ShipPowerOn : MonoBehaviour
    {
        [SerializeField] private GameObject obstacles;
        [SerializeField] private GameObject lights;
        [SerializeField] private Animator doorAnimator;

        public bool isPowerOn = false;

        private void Awake()
        {
            obstacles.SetActive(false);
            lights.SetActive(false);
        }

        public void turnShipOn()
        {
            obstacles.SetActive(true);
            lights.SetActive(true);
            doorAnimator.Play("Closing");
            isPowerOn = true;
            AudioManager.PlayAudio("ShipPowerOn", 1, 1, false);
        }
    }
}
