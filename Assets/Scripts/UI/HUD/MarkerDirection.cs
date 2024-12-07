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

    private Camera mainCamera;
    private void Start()
    {
        currentMarker = GameObject.FindGameObjectWithTag("Marker");
        player = GameObject.FindGameObjectWithTag("Player");
        mainCamera = Camera.main;
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
        float widthMultiplier = Screen.width / 1920f;
        float heightMultiplier = Screen.height / 1080f;
        Vector3 markerPos = currentMarker.transform.position;
        float widthOffset = widthMultiplier * 30;
        float heightOffset = heightMultiplier * 5;
        float minX = -(Screen.width / 2) + markerArrow.GetPixelAdjustedRect().width * widthMultiplier / 2 + widthOffset;
        float maxX = (Screen.width / 2) - markerArrow.GetPixelAdjustedRect().width * widthMultiplier / 2 - widthOffset;
    
        float minY = -(Screen.height / 2) + markerArrow.GetPixelAdjustedRect().height * heightMultiplier / 2 + heightOffset;
        float maxY = (Screen.height / 2) - markerArrow.GetPixelAdjustedRect().height * heightMultiplier / 2 - heightOffset;
    
        Vector2 pos = Camera.main.WorldToScreenPoint(markerPos);
        pos.x -= Screen.width / 2;
        pos.y -= Screen.height / 2;
        Vector2 original = pos;
            
        if (Vector3.Dot((markerPos - mainCamera.transform.position).normalized, mainCamera.transform.forward) < 0)
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
            distanceText.text = (int)Vector3.Distance(mainCamera.transform.position, currentMarker.transform.position) + "m";
            markerIcon.transform.localScale = new Vector3(1 * widthMultiplier, 1 * heightMultiplier, 1);
            return;
        }

        markerIcon.enabled = false;
        distance.SetActive(false);
        markerArrow.enabled = true;
        
        bool YEdge = minY + cornerSize > pos.y || maxY - cornerSize < pos.y;
        bool XEdge = minX + cornerSize > pos.x || maxX - cornerSize < pos.x;

        if (YEdge && XEdge)
        {
            minX += 50 * widthMultiplier;
            maxX -= 50 * widthMultiplier;
            minY += 50 * heightMultiplier;
            maxY -= 50 * heightMultiplier;
        }
            
        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);

        var transform1 = markerArrow.transform;
        transform1.localPosition = new Vector3(pos.x, pos.y, 0);
        transform1.up =  (original - pos).normalized;
        transform1.localEulerAngles = new Vector3(0, 0, transform1.localEulerAngles.z);
        transform1.localScale = new Vector3(1 * widthMultiplier, 1 * heightMultiplier, 1);
    }
}
