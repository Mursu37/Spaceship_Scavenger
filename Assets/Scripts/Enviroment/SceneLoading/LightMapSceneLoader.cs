using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LightMapSceneLoader : MonoBehaviour
{
    private LightMapSceneManager lm_sceneManager;
    public ScenePool pool;

    private void Start()
    {
        if (lm_sceneManager == null)
        {
            lm_sceneManager = GameObject.FindObjectOfType<LightMapSceneManager>();
        }
    }

    public void LoadLightMapScene(int poolNumber)
    {

        int i = poolNumber;

        if (poolNumber < 0)
        {
            i = 0;
        }
        else if (poolNumber >= pool.sceneNames.Length)
        {
            i = pool.sceneNames.Length - 1;
        }

        if (lm_sceneManager != null)
        {
            lm_sceneManager.LoadSceneFromPool(pool.sceneNames[i]);
            Debug.Log(pool.sceneNames[i]);
        }
        else
        {
            throw new System.Exception("Scene is Missing a LightMapSceneManager");
        }
    }

}
