using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosivesSpawner : MonoBehaviour
{
    public GameObject explosive;
    public Transform spawnPoint;
    public float explosiveForce = 15f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            SpawnExplosives();
        }
    }

    private void SpawnExplosives()
    {
        GameObject explosiveObj = Instantiate(explosive, spawnPoint.position, Quaternion.identity);
        explosiveObj.GetComponent<Rigidbody>().AddForce(spawnPoint.forward * explosiveForce, ForceMode.Impulse);
    }
}
