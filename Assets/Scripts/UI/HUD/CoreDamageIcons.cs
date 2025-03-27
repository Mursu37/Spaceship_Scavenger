using System.Collections;
using UnityEngine;

public class CoreDamageIcons : MonoBehaviour
{
    [SerializeField] private float duration = 3f;

    [SerializeField] private GameObject damageIcon;
    [SerializeField] private GameObject warningIcon;

    public void ShowIcons()
    {
        StopAllCoroutines();
        StartCoroutine(HandleIcons());
    }

    private IEnumerator HandleIcons()
    {
        damageIcon.SetActive(true);

        float blinkInterval = 0.2f;
        float elapsedTime = 0f;
        
        while (elapsedTime < duration)
        {
            warningIcon.SetActive(!warningIcon.activeSelf);
            yield return new WaitForSeconds(blinkInterval);
            elapsedTime += blinkInterval;
        }

        warningIcon.SetActive(false);
        damageIcon.SetActive(false);
    }
}
