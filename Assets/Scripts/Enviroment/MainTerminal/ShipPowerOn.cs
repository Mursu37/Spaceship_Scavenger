using System;
using UnityEngine;

namespace Enviroment.MainTerminal
{
    public class ShipPowerOn : MonoBehaviour
    {
        private MeltdownPhase meltdownPhase;

        [SerializeField] private GameObject obstacles;
        [SerializeField] private GameObject lights;
        [SerializeField] private Animator doorAnimator;

        private void Awake()
        {
            obstacles.SetActive(false);
            lights.SetActive(false);

            meltdownPhase = GetComponent<MeltdownPhase>();
        }

        public void turnShipOn()
        {
            obstacles.SetActive(true);
            lights.SetActive(true);
            meltdownPhase.enabled = true;
            doorAnimator.Play("Closing");
        }
    }
}
