using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    public static DifficultyManager instance;

    public static bool easyLevelSelected = false;
    public static bool difficultLevelSelected = false;

    [SerializeField] private GameObject easyLevel;
    [SerializeField] private GameObject difficultLevel;

    private void Awake()
    {
        instance = this;

        if (easyLevelSelected)
        {
            easyLevel.gameObject.SetActive(true);
        }

        if (difficultLevelSelected)
        {
            difficultLevel.gameObject.SetActive(true);
        }
    }
}
