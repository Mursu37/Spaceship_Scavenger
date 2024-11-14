using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthUi : MonoBehaviour
{
    private PlayerHealth playerHealth;
    private float healthCount;

    [SerializeField] private Image currentHealthMeter; 
    [SerializeField] private Image delayedHealthMeter; 
    [SerializeField] private GameObject playerObject;
    [SerializeField] private float delaySpeed = 0.5f; 

    private void Awake()
    {
        playerHealth = playerObject.GetComponent<PlayerHealth>();
    }

    private void Update()
    {
        
        healthCount = playerHealth.currentHealth;
        currentHealthMeter.fillAmount = healthCount / 100f;

        
        if (delayedHealthMeter.fillAmount > currentHealthMeter.fillAmount)
        {
            
            delayedHealthMeter.fillAmount = Mathf.MoveTowards(
                delayedHealthMeter.fillAmount,
                currentHealthMeter.fillAmount,
                delaySpeed * Time.deltaTime
            );
        }
        else
        {
            delayedHealthMeter.fillAmount = currentHealthMeter.fillAmount;
        }
    }
}

