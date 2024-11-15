using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class HighlightCuttable : MonoBehaviour
{
    [SerializeField] private GameObject highlightObjectPrefab;
    [SerializeField] private int range;
    
    [SerializeField] private Cutting cutting;
    private float tolerance;
    private float distance;
    private Collider currentlyCuttable;
    
    private List<Collider> currentlyInRange;
    private Dictionary<Collider, GameObject> highlights;

    private Camera mainCamera;

    private void Start()
    {
        highlights = new Dictionary<Collider, GameObject>();
        currentlyInRange = new List<Collider>();
        tolerance = cutting.Tolerance;
        distance = cutting.RayDistance;
        mainCamera = Camera.main;
    }
    
    private void Update()
    {
        
        var cuttables = Physics.OverlapSphere(transform.position, range, LayerMask.GetMask("Cuttable"));
        var outOfRange = new List<Collider>();
        currentlyInRange.Clear();
        currentlyInRange.AddRange(cuttables);
        
        foreach (var cuttable in highlights)
        {
            // if cuttable is in range remove it from list keep it in dictionary
            if (currentlyInRange.Contains(cuttable.Key)) currentlyInRange.Remove(cuttable.Key);
            else
            {
                // if cuttable is in dictionary but not in range add it to separate list that we loop over later to
                // remove these objects from dictionary. Cannot do in this loop or will cause unintended side-effects
                outOfRange.Add(cuttable.Key);
            }
        }

        // remove out of range objects from dictionary and delete the highlight object
        foreach (var cuttable in outOfRange)
        {
            if (highlights[cuttable] != null) Destroy(highlights[cuttable]);
            highlights.Remove(cuttable);
        }

        // currently in range but not in dictionary are given highlight object and added to dictionary
        foreach (var cuttable in currentlyInRange)
        {
            var highlight = Instantiate(highlightObjectPrefab, cuttable.transform);
                
            var original = highlight.transform.localPosition;
            
            // calculate which side of the cuttable object highlighting should be on
            // side note after doing this math... Does our game even have double sided doors??? should have probably double checked
            highlight.transform.localPosition = new Vector3(original.x, original.y, original.z + 10);
            var distPos = Vector3.Distance(transform.position, highlight.transform.position);
                
            highlight.transform.localPosition = new Vector3(original.x, original.y, original.z - 10);
            var distNeg = Vector3.Distance(transform.position, highlight.transform.position);

            if (distPos > distNeg)
            {
                highlight.transform.localPosition = new Vector3(original.x, original.y,
                    original.z - cuttable.bounds.extents.z * (1 / highlight.transform.lossyScale.z));
            }
            else
            {
                highlight.transform.localPosition = new Vector3(original.x, original.y,
                    original.z + cuttable.bounds.extents.z * (1 / highlight.transform.lossyScale.z));
            }
            highlights.Add(cuttable, highlight);
        }

        // Change highlight visual look if it's currently cuttable
        if (highlights.Count > 0 && cutting.enabled)
        {
            // by default assumes that object is no longer cuttable
            if (currentlyCuttable != null && highlights.ContainsKey(currentlyCuttable))
            {
                highlights[currentlyCuttable].GetComponent<CuttableSwitch>().NotCuttable();
                currentlyCuttable = null;
            }
            if (Physics.Raycast(new Ray(mainCamera.transform.position, mainCamera.transform.forward), out RaycastHit hit, distance,
                    ~LayerMask.GetMask("Player")))
            {
                if (hit.transform.CompareTag("Cuttable") && highlights.ContainsKey(hit.collider) && cutting.AreAnglesClose(cutting.transform, hit.transform, tolerance))
                {
                    highlights[hit.collider].GetComponent<CuttableSwitch>().Cuttable();
                    currentlyCuttable = hit.collider;
                }
            }
        }
    }
}
