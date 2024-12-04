using Unity.VisualScripting;
using UnityEngine;

namespace UI.HUD
{
    public class MarkerTrigger : MonoBehaviour
    {
        public int id;

        [SerializeField] private GameObject nextMarker;

        private void Start()
        {
            if (CheckpointManager.checkpointReached || CheckpointManager.engineRoomReached)
            {
                if (CheckpointManager.currentActiveMarkerId != id)
                {
                    gameObject.SetActive(false);
                }
            }
            else if (id != 1)
            {
                gameObject.SetActive(false);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            NextMarker();
        }

        public void NextMarker()
        {
            if (nextMarker != null) nextMarker.SetActive(true);
            transform.gameObject.SetActive(false);
        }
    }
}
