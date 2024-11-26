using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ExplosionMaterialDriver : MonoBehaviour
{
    [SerializeField]
    private Material explosionMaterial;
    public float targetValue = 2f;
    public float duration = 1f;
    private float currentValue = 0f;
    private Coroutine coroutine;


    // Start is called before the first frame update

    void OnEnable()
    {
        StartExplosionCoroutine();
    }

    private void OnDisable()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
        }
    }


    public void StartExplosionCoroutine()
    {
        currentValue = 0f;
        explosionMaterial.SetFloat("_timeNode", currentValue);
        if (coroutine == null)
        {
            coroutine = StartCoroutine(IncreaseFloat());
        }

    }

    private IEnumerator IncreaseFloat()
    {
        float elapsedTime = 0f;
        float initialvalue = currentValue;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            currentValue = Mathf.Lerp(initialvalue, targetValue, elapsedTime / duration);

            explosionMaterial.SetFloat("_timeNode", currentValue);

            yield return null;
        }

        currentValue = targetValue;
        explosionMaterial.SetFloat("_timeNode", currentValue);
    }

}
