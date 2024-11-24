using UnityEngine;
using TMPro; // For TextMeshPro UI elements

public class CuttingUI : MonoBehaviour
{
    [SerializeField] private Cutting cuttingScript; 
    [SerializeField] private TextMeshProUGUI detectionText; 
    [SerializeField] private TextMeshProUGUI alignmentText; 
   // [SerializeField] private TextMeshProUGUI statusText; 

    private void Update()
    {
        if (cuttingScript == null)
            return;

        // Object detection status
        if (detectionText != null)
        {
            detectionText.text = cuttingScript.IsObjectDetected() ? "DETECTED" : " ";
        }

        // Alignment status
        if (alignmentText != null)
        {
            if (cuttingScript.IsObjectDetected())
            {
                alignmentText.text = cuttingScript.IsCutAligned() ? "YES" : "NO";
            }
        }
    }
}
