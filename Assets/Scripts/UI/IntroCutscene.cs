using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroCutscene : MonoBehaviour
{
    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        StartCoroutine(PlayVideo());
    }

    private IEnumerator PlayVideo()
    {
        yield return new WaitForSeconds(41f);
        SceneManager.LoadSceneAsync("MainGame");
    }

    private void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            SceneManager.LoadSceneAsync("MainGame");
        }
    }
}
