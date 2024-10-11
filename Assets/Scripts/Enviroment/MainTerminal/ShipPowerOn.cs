using System;
using UnityEngine;

namespace Enviroment.MainTerminal
{
    public class ShipPowerOn : MonoBehaviour
    {
        [SerializeField] private GameObject obstacles;
        [SerializeField] private GameObject lights;

        private void Awake()
        {
            obstacles.SetActive(false);
            lights.SetActive(false);
        }

        public void turnShipOn()
        {
            obstacles.SetActive(true);
            lights.SetActive(true);
        }
    }
}
