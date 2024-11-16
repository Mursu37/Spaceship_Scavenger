using System;
using TMPro;
using UnityEngine;

namespace UI.HUD
{
    public class Marker : MonoBehaviour
    {
        [SerializeField] private TMP_Text distanceText;
        private GameObject player;
        private Vector3 scale;
        private void Start()
        {
            player = GameObject.FindWithTag("Player");
            scale = transform.localScale;
        }

        void Update()
        {
            float distance = Vector3.Distance(player.transform.position, transform.position);
            distanceText.text = (int)distance + "m";
            transform.LookAt(player.transform);
            if(distance > 10)transform.localScale = scale * (distance * 0.9f / 10);
        }
    }
}
