using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeltdownMusic : MonoBehaviour
{
    [SerializeField] private string meltdownIntro;
    [SerializeField] private string meltdownLayer1;
    [SerializeField] private string meltdownLayer2;
    [SerializeField] private string meltdownLayer3;


    // Start is called before the first frame update
    void Start()
    {
        this.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActivateMeltdownMusic()
    {
        this.enabled = true;
        AudioManager.PlayAudio(meltdownIntro, 1, 1, false);
    }
}
