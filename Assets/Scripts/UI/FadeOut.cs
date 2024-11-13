using System.Linq;
using UnityEngine;

public class FadeOut : MonoBehaviour
{
    [SerializeField] private bool canFadeOut = false;
    [SerializeField] private float fadeTime = 1f;
    [SerializeField] CanvasGroup[] groups;
    public bool allFadedOut = false;

    public void StartFadeOut()
    {
        canFadeOut = true;
    }

    private void Update()
    {
        if (canFadeOut)
        {
            foreach (var group in groups)
            {
                if (group.alpha > 0f)
                {
                    group.alpha -= fadeTime * Time.deltaTime;

                    if (group.alpha < 0f)
                    {
                        group.alpha = 0f;
                    }

                    if (group.alpha > 0f)
                    {
                        break;
                    }
                }
            }

            allFadedOut = groups.All(g => g.alpha <= 0f);
            if (allFadedOut)
            {
                canFadeOut = false;
            }
        }
    }
}
