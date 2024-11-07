using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOffTurnOn : MonoBehaviour
{
    private laser laser;
    private LineRenderer lineRenderer;

    [SerializeField] GameObject right;
    [SerializeField] GameObject left;

    float enableDuration = 2f;
    float disableDuration = 4f;

    void Start()
    {
        laser = GetComponent<laser>();
        lineRenderer = GetComponent<LineRenderer>();
        StartCoroutine(ToggleCoroutine());
    }

    IEnumerator ToggleCoroutine()
    {
        while (true)
        {
            while (true)
            {
                // Enable the right GameObject and disable the left
                right.SetActive(true);
                left.SetActive(false);

                // Wait for enableDuration
                yield return new WaitForSeconds(enableDuration);

                // Disable the right GameObject
                right.SetActive(false);

                // Wait for disableDuration
                yield return new WaitForSeconds(disableDuration);

                // Enable the left GameObject and disable the right
                left.SetActive(true);
                right.SetActive(false);

                // Wait for enableDuration
                yield return new WaitForSeconds(enableDuration);

                // Disable the left GameObject
                left.SetActive(false);

                // Wait for disableDuration before starting the loop again
                yield return new WaitForSeconds(disableDuration);
            }
        }
    }
}
