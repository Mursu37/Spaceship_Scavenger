using UnityEngine;

namespace UI.HUD
{
    public class MarkerTrigger : MonoBehaviour
    {
        [SerializeField] private GameObject nextMarker;

        private void OnTriggerEnter(Collider other)
        {
            if (nextMarker != null) nextMarker.SetActive(true);
            transform.gameObject.SetActive(false);
        }
    }
}
