using TMPro;
using UnityEngine;

public class SmoothTextHighlight : MonoBehaviour
{
    public bool canHighlight = false;
    [SerializeField] private float fadeTime = 1f;
    [SerializeField] private TextMeshProUGUI textMeshPro;
    private Material mat;
    private float glowPower = 0f;
    private bool highlighted = false;

    public void StartHiglight()
    {
        canHighlight = true;

        mat = new Material(textMeshPro.fontSharedMaterial);
        textMeshPro.fontMaterial = mat;

        mat.EnableKeyword("GLOW_ON");
        mat.SetFloat("_GlowPower", 0f);
    }

    private void Update()
    {
        if (canHighlight && !highlighted)
        {
            glowPower += fadeTime * Time.deltaTime;
            glowPower = Mathf.Clamp(glowPower, 0f, 1f);

            mat.SetFloat("_GlowPower", glowPower);

            if (glowPower >= 1f)
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
