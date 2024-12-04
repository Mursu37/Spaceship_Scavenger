using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using UnityEditor;

public class LightMapSceneManager : MonoBehaviour
{
    [SerializeField]
    private string startScene;

    private string currentScene;
    private AsyncOperation asyncOperation;
    private AsyncOperation unloadOperation;
    private bool isSceneReady = false;
    private string loadedSceneInBuffer;

    public static LightMapSceneManager instance;
    public ScenePool pool;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        if (startScene != null)
        {
            LoadSceneFromPool(startScene);
        }
    }

    public void PreLoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    public void UnloadScene(string sceneName)
    {
        StartCoroutine(UnloadSceneAsync(sceneName));
    }

    public IEnumerator LoadSceneAsync(string sceneName)
    {
        //Start loading the scene
        asyncOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        asyncOperation.allowSceneActivation = false;

        while (!asyncOperation.isDone)
        {
            //Check if the scene has been loaded but not activated
            if (asyncOperation.progress >= 0.9f && !isSceneReady)
            {
                Debug.Log("Scene is preloaded and ready to activate.");
                loadedSceneInBuffer = sceneName;
                isSceneReady = true;
            }


            yield return null; // Wait for the next frame
        }
    }

    private IEnumerator UnloadSceneAsync(string sceneName)
    {
        unloadOperation = SceneManager.UnloadSceneAsync(sceneName);

        if (unloadOperation == null)
        {
            Debug.LogWarning($"Scene '{sceneName}' is not loaded or cannot be unloaded.");
            yield break;
        }

        while (!unloadOperation.isDone)
        {
            Debug.Log($"Unloading scene '{sceneName}'... Progress: {unloadOperation.progress * 100}%");
            yield return null; // Wait for the next frame
        }

        ClearLightmaps();
        Debug.Log($"Scene '{sceneName}' has been successfully unloaded.");
    }

    public void LoadSceneFromPool(string newScene)
    {

        foreach (var sceneName in pool.sceneNames)
        {
            if (sceneName != newScene && SceneManager.GetSceneByName(sceneName).isLoaded)
            {
                UnloadScene(sceneName);
                Debug.Log(sceneName + " " + "- started asynchronous unload.");
            }
        }

        StartCoroutine(LoadNewSceneWithDelay(newScene, 0.5f));

    }

    private IEnumerator LoadNewSceneWithDelay(string newScene, float delay)
    {   // Wait for a short delay
        yield return new WaitForSecondsRealtime(delay);

        if (!SceneManager.GetSceneByName(newScene).isLoaded)
        {
            if (isSceneReady && loadedSceneInBuffer == newScene)
            {
                Debug.Log("Activating buffered scene...");
                asyncOperation.allowSceneActivation = true;
            }
            else
            {
                SceneManager.LoadScene(newScene, LoadSceneMode.Additive);
                currentScene = newScene;
            }

            //SceneManager.SetActiveScene(SceneManager.GetSceneByName(newScene));

        }
    }

    void ClearLightmaps()
    {
        LightmapSettings.lightmaps = new LightmapData[0];
        RenderSettings.skybox = null; // Optional: Clear skybox to avoid visual inconsistencies
    }

}
