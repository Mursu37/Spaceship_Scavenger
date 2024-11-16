using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class LightMapSceneManager : MonoBehaviour
{
    private string currentScene;
    private List<string> previousScenes = new List<string>();
    [SerializeField]
    private string path;

    [SerializeField]
    private string startScene;

    public ScenePool pool;

    // Start is called before the first frame update
    void Start()
    {
        if (startScene != null)
        {
            LoadSceneFromPool(startScene);
        }
    }

    public void LoadSceneFromPool(string newScene)
    {

        foreach (var sceneName in pool.sceneNames)
        {
            if (sceneName != newScene && SceneManager.GetSceneByName(sceneName).isLoaded)
            {
                SceneManager.UnloadSceneAsync(sceneName);
                Debug.Log(sceneName + " " + "unloaded.");
            }
        }

        if (!SceneManager.GetSceneByName(newScene).isLoaded)
        {
            SceneManager.LoadScene(newScene, LoadSceneMode.Additive);
            currentScene = newScene;
        }

    }

}
