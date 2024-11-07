using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PowerOn : MonoBehaviour
{
    public static bool isPowerOn = false;

    private Interpreter interpreter;
    [SerializeField] private TMP_Text commandLineText;

    // Start is called before the first frame update
    void Start()
    {
        interpreter = GetComponent<Interpreter>();
        commandLineText.text = "This computer requires the main power to fully operate the Reactor Station Room.";
    }

    // Update is called once per frame
    void Update()
    {
        if (isPowerOn)
        {
            interpreter.enabled = true;
            enabled = false;
        }
    }
}
