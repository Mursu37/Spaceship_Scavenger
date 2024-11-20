using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementSounds : MonoBehaviour
{
    private bool isThrusting = false;
    
    [SerializeField] private float minPitch = 0.8f;
    [SerializeField] private float maxPitch = 1.2f; 

    // Speed threshold for playing stopping sound
    [SerializeField] private float speedThreshold = 1.0f; // Minimum speed to play stopping sound
    private Rigidbody rb; // Reference to the Rigidbody component

    //Continous input time threshold for playing thruster hiss
    [SerializeField] private float inputTimeThreshold = 2.0f;
    private float inputTimer = 0.0f; //timer to track continuous input



    private void Start()
    {

        rb = GetComponentInParent<Rigidbody>(); // Get the Rigidbody component from the parent object
    }

    
    private void Update()
    {
        float currentSpeed = rb.velocity.magnitude; // get the speed from rb component
        // Are any movement keys pressed

        if (Input.GetButton("Horizontal") || Input.GetButton("Vertical") || Input.GetButton("Roll") || Input.GetButton("Ascend"))
        {
            inputTimer += Time.deltaTime;
            if (!isThrusting)
            {
                StartThrusting();
            }
        }
        else
        {
            if (isThrusting)
            {
                StopThrusting();
            }

            //Play thruster hiss if input has been continuous for enough time
            if (inputTimer >= inputTimeThreshold)
            {
                PlayThrusterHiss();
            }

            inputTimer = 0f; //reset timer
        }

        // Check if left Ctrl is pressed and if the player's speed is high enough
        if (Input.GetButton("Break") && currentSpeed > speedThreshold)
        {
            PlaystoppingSound();
        }

    }

    
    private void StartThrusting()
    {
        isThrusting = true;
        if (!AudioManager.IsPlaying("Thruster"))
        {
            AudioManager.PlayAudio("Thruster", 1, Random.Range(minPitch, maxPitch));

        }
    }

    // Stop playing thruster audio when keys are released
    private void StopThrusting()
    {
        isThrusting = false;
        if (AudioManager.IsPlaying("Thruster"))
        {
            AudioManager.StopAudio("Thruster");

        }
    }

    //play stopping sound
    private void PlaystoppingSound()
    {
        if (!AudioManager.IsPlaying("ThrusterStop"))
        {
            AudioManager.PlayAudio("ThrusterStop", 1, 1, false);
        }
    }

    //Play thruster hiss
    private void PlayThrusterHiss()
    {
        if (!AudioManager.IsPlaying("ThrusterHiss"))
        {
            AudioManager.PlayAudio("ThrusterHiss", 1, 1, false);
        }
    }
}
