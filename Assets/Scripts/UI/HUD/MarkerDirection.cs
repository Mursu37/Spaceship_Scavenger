using UnityEngine;
using UnityEngine.UI;

public class MarkerDirection : MonoBehaviour
{
    private GameObject currentMarker;
    [SerializeField] private Image markerArrow;
    private void Start()
    {
        currentMarker = GameObject.FindGameObjectWithTag("Marker");
    }

    private void Update()
    {
        Vector3 markerPos = currentMarker.transform.position;
        
        float minX = markerArrow.GetPixelAdjustedRect().width / 2;
        float maxX = Screen.width - minX;

        float minY = markerArrow.GetPixelAdjustedRect().height / 2;
        float maxY = Screen.height - minY;

        Vector2 pos = Camera.main.WorldToScreenPoint(markerPos);
        Vector2 original = pos;
        
        if (Vector3.Dot((markerPos - Camera.main.transform.position).normalized, Camera.main.transform.forward) < 0)
        {
            if (pos.x < Screen.width / 2)
            {
                pos.x = maxX;
            }
            else
            {
                pos.x = minX;
            }
            if (pos.y < Screen.height / 2)
            {
                pos.y = maxY;
            }
            else
            {
                pos.y = minY;
            }
        }
        
        if ((minX < pos.x && maxX > pos.x) && (minY < pos.y && maxY > pos.y))
        {
            markerArrow.enabled = false;
            return;
        }
        markerArrow.enabled = true;
        
        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);

        markerArrow.transform.position = pos;
        markerArrow.transform.up = (original - pos).normalized;
    }
}
