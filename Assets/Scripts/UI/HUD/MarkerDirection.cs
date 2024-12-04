using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MarkerDirection : MonoBehaviour
{
    private GameObject currentMarker;
    private GameObject player;
    private float cornerSize = 50f;
    
    [SerializeField] private Image markerArrow;
    [SerializeField] private Image markerIcon;
    [SerializeField] private GameObject distance;
    [SerializeField] private TMP_Text distanceText;
    private void Start()
    {
        currentMarker = GameObject.FindGameObjectWithTag("Marker");
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void LateUpdate()
    {
        currentMarker = GameObject.FindGameObjectWithTag("Marker");
        if (currentMarker == null)
        {
            markerArrow.enabled = false;
            markerIcon.enabled = false;
            distance.SetActive(false);
            return;
        }
        Vector3 markerPos = currentMarker.transform.position;
            
        float minX = -(Screen.width / 2) + markerArrow.GetPixelAdjustedRect().width / 2 + 30;
        float maxX = (Screen.width / 2) - markerArrow.GetPixelAdjustedRect().width / 2 - 30;
    
        float minY = -(Screen.height / 2) + markerArrow.GetPixelAdjustedRect().height / 2 + 5;
        float maxY = (Screen.height / 2) - markerArrow.GetPixelAdjustedRect().height / 2 - 5;
    
        Vector2 pos = Camera.main.WorldToScreenPoint(markerPos);
        pos.x -= Screen.width / 2;
        pos.y -= Screen.height / 2;
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
            markerIcon.enabled = true;
            distance.SetActive(true);
            markerIcon.transform.localPosition = new Vector3(pos.x, pos.y + 30, 0);
            markerIcon.transform.localEulerAngles = new Vector3(0, 0, -(player.transform.eulerAngles.z));
            distanceText.text = (int)Vector3.Distance(Camera.main.transform.position, currentMarker.transform.position) + "m";
            return;
        }

        markerIcon.enabled = false;
        distance.SetActive(false);
        markerArrow.enabled = true;
        
        bool YEdge = minY + cornerSize > pos.y || maxY - cornerSize < pos.y;
        bool XEdge = minX + cornerSize > pos.x || maxX - cornerSize < pos.x;

        if (YEdge && XEdge)
        {
            minX += 50;
            maxX -= 50;
            minY += 50;
            maxY -= 50;
        }
            
        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);
            
        markerArrow.transform.localPosition = new Vector3(pos.x, pos.y, 0);
        markerArrow.transform.up =  (original - pos).normalized;
        markerArrow.transform.localEulerAngles = new Vector3(0, 0, markerArrow.transform.localEulerAngles.z);
    }
}
