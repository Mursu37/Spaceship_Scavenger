using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AmbientMusic : MonoBehaviour
{
    [SerializeField] private string[] ambientMusicTracks;
    private string lastPlayedTrackName; 
    private const float defaultTrackLength = 120f;

    private string selectedTrackName;


    // Start is called before the first frame update
    private void Start()
    {
     string activeSceneName = SceneManager.GetActiveScene().name;
    
        if (activeSceneName == "MainGame")
        {
            PlayAmbientMusic();
        }
        else
        {
            Debug.LogWarning($"Active scene is '{activeSceneName}', expected 'MainGame'. Ambient music will not play.");
        }
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    private void PlayAmbientMusic()
    {
        if(ambientMusicTracks.Length > 0)
        {
            selectedTrackName = SelectRandomTrackName();

            AudioManager.PlayAudio(selectedTrackName, 1f, 1f, false);

            lastPlayedTrackName = selectedTrackName;

            StartCoroutine(PlayNextAmbientTrackAfterDelay());
        }
    }

    private string SelectRandomTrackName()
    {
        string selectedTrackName;
        do
        {
            selectedTrackName = ambientMusicTracks[Random.Range(0, ambientMusicTracks.Length)];
        } while (selectedTrackName == lastPlayedTrackName); // Ensure a different track is selected

        return selectedTrackName;
    }

    private IEnumerator PlayNextAmbientTrackAfterDelay()
    {
        // Wait for the current track to finish playing
        yield return new WaitForSeconds(defaultTrackLength);

        // Wait for a random duration of silence
        float silenceDuration = Random.Range(20f, 25f);
        yield return new WaitForSeconds(silenceDuration);

        // Play the next random track
        PlayAmbientMusic();
    }

    public void StopAmbientMusic()
    {
        if (!string.IsNullOrEmpty(selectedTrackName))
        {
            AudioManager.StopAudio(selectedTrackName); // Stop any currently playing music
        }

        this.enabled = false; // Disable this script
    }

}
