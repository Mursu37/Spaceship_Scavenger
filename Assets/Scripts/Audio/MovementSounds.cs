using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementSounds : MonoBehaviour
{
[SerializeField] private AudioSource audioSource1; // Assign these in the inspector
[SerializeField] private AudioSource audioSource2;
[SerializeField] private AudioSource stoppingSound;
[SerializeField] private AudioSource thrusterHiss;
private bool isThrusting = false;
    
// For Audio Source 1 = player_suit_thruster
 [SerializeField] private float minPitch = 0.8f;
 [SerializeField] private float maxPitch = 1.2f; 

 // For Audio Source 2 = player_suit_thruster_layer
 [SerializeField] private float fadeInSpeed = 0.2f;
 [SerializeField] private float maxVolume = 1.0f;

 // Speed threshold for playing stopping sound
 [SerializeField] private float speedThreshold = 1.0f; // Minimum speed to play stopping sound
 private Rigidbody rb; // Reference to the Rigidbody component

 //Continous input time threshold for playing thruster hiss
 [SerializeField] private float inputTimeThreshold = 2.0f;
 private float inputTimer = 0.0f; //timer to track continuous input

    private void Start()
    {
        audioSource2.volume = 0f;

        if(audioSource1 == null || audioSource2 == null || stoppingSound == null)
        {
            Debug.LogError("Please assign both AudioSources in the Inspector.");
        }

        rb = GetComponentInParent<Rigidbody>(); // Get the Rigidbody component from the parent object
    }

    
    private void Update()
    {
        float currentSpeed = rb.velocity.magnitude; // get the speed from rb component
        // Are any movement keys pressed
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.Space))
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

        if (audioSource2.isPlaying && audioSource2.volume < maxVolume)
        {
            audioSource2.volume += fadeInSpeed * Time.deltaTime;
        }

        // Check if left Ctrl is pressed and if the player's speed is high enough
        if (Input.GetKeyDown(KeyCode.LeftControl) && currentSpeed > speedThreshold)
        {
            PlaystoppingSound();
        }

    }

    
    private void StartThrusting()
    {
        isThrusting = true;
        if (!audioSource1.isPlaying || !audioSource2.isPlaying)
        {
            audioSource1.pitch = Random.Range(minPitch, maxPitch);
            audioSource1.Play();
            audioSource2.Play();
        }
    }

    // Stop playing thruster audio when keys are released
    private void StopThrusting()
    {
        isThrusting = false;
        if (audioSource1.isPlaying || audioSource2.isPlaying)
        {
            audioSource1.Stop();
            audioSource2.Stop();
            audioSource2.volume = 0f;
        }
    }

    //play stopping sound
    private void PlaystoppingSound()
    {
        if (!stoppingSound.isPlaying)
        {
            stoppingSound.pitch = Random.Range(minPitch, maxPitch);
            stoppingSound.Play();
        }
    }

    //Play thruster hiss
    private void PlayThrusterHiss()
    {
        if (!thrusterHiss.isPlaying)
        {
            thrusterHiss.Play();
        }
    }
}
