using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultitoolSway : MonoBehaviour
{
    [SerializeField] private float swayAmountA = 1;
    [SerializeField] private float swayAmountB = 2;
    [SerializeField] private float swayScale = 600;
    [SerializeField] private float swayLerpSpeed = 0;
    [SerializeField] private float swayTime;
    private Vector3 swayPosition;

    // Start is called before the first frame update
    void Start()
    {
        swayPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        CalculateSway();
    }

    private void CalculateSway()
    {
        Vector3 targetPosition = LissajousCurve(swayTime, swayAmountA, swayAmountB) / swayScale;

        swayPosition = Vector3.Lerp(swayPosition, targetPosition, Time.smoothDeltaTime * swayLerpSpeed);
    }

    private Vector3 LissajousCurve(float Time, float A, float B)
    {
        return new Vector3(Mathf.Sin(Time), A * Mathf.Sin(B * Time + Mathf.PI));
    }
}
