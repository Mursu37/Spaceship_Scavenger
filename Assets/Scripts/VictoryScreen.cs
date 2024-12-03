using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VictoryScreen : MonoBehaviour
{
    [SerializeField] Material material;
    [SerializeField] VideoPlayer videoPlayer;
    [SerializeField] GameObject screen;

    public void PlayVideo()
    {
        screen.GetComponent<MeshRenderer>().material = material;
        videoPlayer.Play();
    }
}
