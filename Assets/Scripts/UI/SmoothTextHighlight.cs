using TMPro;
using UnityEngine;

public class SmoothTextHighlight : MonoBehaviour
{
    public bool canHighlight = false;
    [SerializeField] private float fadeTime = 1f;
    [SerializeField] private TextMeshProUGUI textMeshPro;
    [SerializeField] private Color highlightedColor;
    private Color originalColor;
    private float t = 0f;
    private bool highlighted = false;

    private void Start()
    {
        if (textMeshPro != null)
        {
            originalColor = textMeshPro.color;
        }
    }

    public void StartHighlight()
    {
        canHighlight = true;
        t = 0f;
    }

    private void Update()
    {
        if (canHighlight && !highlighted)
        {
            t += Time.deltaTime / fadeTime;
            textMeshPro.color = Color.Lerp(originalColor, highlightedColor, t);

            if (t >= 1f)
            {
                highlighted = true;
                canHighlight = false;
            }
        }

        if (highlighted)
        {
            FadeOut fadeOut = GetComponent<FadeOut>();
            if (fadeOut != null)
            {
                fadeOut.StartFadeOut();
            }
        }
    }
}
