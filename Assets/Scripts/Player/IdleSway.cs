using UnityEngine;

public class IdleSway : MonoBehaviour
{
    [SerializeField] private float swayAmountA = 1;
    [SerializeField] private float swayAmountB = 2;
    [SerializeField] private float swayScale = 600;
    [SerializeField] private float swayLerpSpeed = 0;
    private float swayTime;
    private Vector3 swayPosition;

    private void Update()
    {
        CalculateSway();
    }

    private void CalculateSway()
    {
        if (swayScale != 0f)
        {
            swayTime += Time.deltaTime;

            if (swayTime > 6.3f)
            {
                swayTime = 0f;
            }

            Vector3 targetPosition = LissajousCurve(swayTime, swayAmountA, swayAmountB) / swayScale;

            swayPosition = Vector3.Lerp(swayPosition, targetPosition, swayLerpSpeed * Time.deltaTime);

            transform.localPosition = swayPosition;
        }
    }

    private Vector3 LissajousCurve(float Time, float A, float B)
    {
        return new Vector3(Mathf.Sin(Time), A * Mathf.Sin(B * Time + Mathf.PI));
    }
}
