using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class XRayRadius : MonoBehaviour
{
    [SerializeField] private float expansionRate;
    [SerializeField] private float maxExpansionDistance;
    [SerializeField] private bool turnWallsOff;
    
    [SerializeField] private float timeBeforeExpiration;
    private float timer;
    
    private int xRayLayer;
    //private List<GameObject> xRayObjects;
    private Dictionary<GameObject, int> xRayObjects;
    private Transform sphereScale;
    private TogglexRay player;

    private void Start()
    {
        //xRayObjects = new List<GameObject>();
        xRayObjects = new Dictionary<GameObject, int>();
        timer = 0;
        xRayLayer = LayerMask.NameToLayer("XRay");

        if (turnWallsOff)
        {
            Camera.main.cullingMask &= ~(1 << LayerMask.NameToLayer("Default"));
        }
    }

    private void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;
        if (timer > timeBeforeExpiration) Destroy(transform.parent.gameObject);
        if (transform.localScale.magnitude < maxExpansionDistance)
        {
            ExpandRadius();
        }
    }

    private void ExpandRadius()
    {
        Vector3 scale = transform.localScale;
        float add = Time.fixedDeltaTime * expansionRate;
        Vector3 newScale = new Vector3(scale.x + add, scale.y + add, scale.z + add);
        transform.localScale = newScale;
    }

    private void OnDestroy()
    {
        foreach (KeyValuePair<GameObject, int> obj in xRayObjects)
        {
            obj.Key.layer = obj.Value;
        }

        if (turnWallsOff)
        {
            Camera.main.cullingMask |= (1 << LayerMask.NameToLayer("Default"));
        }
        
        player.XRayEnded();
    }

    private void OnTriggerEnter (Collider other)
    {
        xRayObjects.Add(other.gameObject, other.gameObject.layer);
        other.gameObject.layer = xRayLayer;
    }

    public void SetOwner(TogglexRay p)
    {
        player = p;
    }
    
    
}
