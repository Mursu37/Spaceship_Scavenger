using System.Linq;
using UnityEngine;

public class FadeIn : MonoBehaviour
{
    public bool canFadeIn = false;
    [SerializeField] private float fadeTime = 1f;
    [SerializeField] CanvasGroup[] groups;
    public bool allFadedIn = false;

    public void StartFadeIn()
    {
        canFadeIn = true;
    }

    private void Update()
    {
        if (canFadeIn)
        {
            foreach (var group in groups)
            {
                if (group.alpha < 1f)
                {
                    group.alpha += fadeTime * Time.deltaTime;

                    if (group.alpha > 1f)
                    {
                        group.alpha = 1f;
                    }

                    if (group.alpha < 1f)
                    {
                        break;
                    }
                }
            }

            allFadedIn = groups.All(g => g.alpha >= 1f);
            if (allFadedIn)
            {
                canFadeIn = false;
            }
        }
    }
}
