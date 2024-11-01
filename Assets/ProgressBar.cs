using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PrograssBar : MonoBehaviour
{
    public GameObject bar;
    public int time = 10;

    private Vector3 initialScale;

    private void Start()
    {
        initialScale = bar.transform.localScale;
        StartCoroutine(RunTimer());
    }

    private IEnumerator RunTimer()
    {
        float elapsedTime = 0;

        while (elapsedTime < time)
        {
            float progress = 1 - (elapsedTime / time);
            bar.transform.localScale = new Vector3(initialScale.x * progress, initialScale.y, initialScale.z);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        bar.transform.localScale = new Vector3(0, initialScale.y, initialScale.z);
    }
}
