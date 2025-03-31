using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingTutorialTrigger : MonoBehaviour
{
    [SerializeField] private CuttingTutorial tutorial;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !CheckpointManager.engineRoomReached && !tutorial.gameObject.GetComponent<FadeOut>().canFadeOut)
        {
            tutorial.enabled = true;
            tutorial.gameObject.GetComponent<FadeIn>().StartFadeIn();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && !CheckpointManager.engineRoomReached && !tutorial.gameObject.GetComponent<FadeIn>().canFadeIn)
        {
            tutorial.enabled = false;
            tutorial.gameObject.GetComponent<FadeOut>().StartFadeOut();
        }
    }
}
