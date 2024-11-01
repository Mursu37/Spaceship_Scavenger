using System.Collections;
using System.Collections.Generic;
using UnityEditor.Profiling;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode()]

public class MeltdownBar : MonoBehaviour
{
    public int maximum;
    public int current;
    public RawImage bar;
    private RectTransform barRectTransform;

    // Start is called before the first frame update
    void Start()
    {
        barRectTransform = bar.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
       GetCurrentFill();
    }

    void GetCurrentFill()
    {
        float fillAmount = (float)current / (float)maximum;
        barRectTransform.localScale = new Vector3(fillAmount, 1, 1);
    }
}
    

