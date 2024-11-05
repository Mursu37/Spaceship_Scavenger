using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class closeCLI : MonoBehaviour
{
    [SerializeField] private GameObject CLI;
    private void Start()
    {
        transform.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        Time.timeScale = 0;
    }

    private void OnDisable()
    {
        Time.timeScale = 1;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.Escape))
        {
            transform.gameObject.SetActive(false);
        }
    }
}
