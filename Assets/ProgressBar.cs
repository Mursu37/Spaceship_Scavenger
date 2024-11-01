using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressBar : MonoBehaviour
{
    public GameObject bar;
    public int time = 900; // 15 minutes in seconds

    private Vector3 initialScale;
    private Vector3 initialPosition;

    private void Start()
    {
        initialScale = bar.transform.localScale;
        initialPosition = bar.transform.localPosition;
        StartCoroutine(RunTimer());
    }

    private IEnumerator RunTimer()
    {
        float elapsedTime = 0;

        while (elapsedTime < time)
        {
            float progress = 1 - (elapsedTime / time);
            bar.transform.localScale = new Vector3(initialScale.x * progress, initialScale.y, initialScale.z);
            bar.transform.localPosition = new Vector3(
                initialPosition.x - (initialScale.x - bar.transform.localScale.x) / 2,
                initialPosition.y,
                initialPosition.z);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        bar.transform.localScale = new Vector3(0, initialScale.y, initialScale.z);
        bar.transform.localPosition = initialPosition - new Vector3(initialScale.x / 2, 0, 0);
    }
}
