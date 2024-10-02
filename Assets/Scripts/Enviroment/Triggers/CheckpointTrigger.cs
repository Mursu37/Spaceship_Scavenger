using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckpointTrigger : MonoBehaviour
{
    [SerializeField] private GameObject respawnPoint;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerPrefs.SetFloat("PlayerPosX", respawnPoint.transform.position.x);
            PlayerPrefs.SetFloat("PlayerPosY", respawnPoint.transform.position.y);
            PlayerPrefs.SetFloat("PlayerPosZ", respawnPoint.transform.position.z);
        }
    }
}
