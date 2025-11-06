using UnityEngine;

public class HideCanvas : MonoBehaviour
{
    private Canvas canvas;
    [SerializeField] private GameObject multitool;

    private void Start()
    {
        canvas = GetComponentInChildren<Canvas>(true);
        if (canvas == null)
        {
            Debug.LogWarning("No Canvas found in children of HideCanvas.");
        }
    }

    private void Update()
    {
        if (Input.GetButtonDown("Hide UI"))
        {
            if (canvas != null)
            {
                canvas.gameObject.SetActive(!canvas.gameObject.activeSelf);
            }

            if (multitool != null)
            {
                multitool.SetActive(!multitool.activeSelf);
            }
        }
    }
}
