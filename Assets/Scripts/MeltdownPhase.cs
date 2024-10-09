using UnityEngine;

public class MeltdownPhase : MonoBehaviour
{
    private bool canSpawn = false;

    private void Awake()
    {
        if (enabled)
        {
            enabled = false;
        }
    }

    private void Start()
    {
        canSpawn = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
