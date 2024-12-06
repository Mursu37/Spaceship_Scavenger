using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttableSwitch : MonoBehaviour
{
    [SerializeField] private GameObject highlightOutside;
    [SerializeField] private GameObject highlightDirection;
    [SerializeField] private Material highlightBlockedMat;

    [SerializeField] private LayerMask layerMask;
    private Camera mainCamera;

    private Material outsideMaterial;
    private Material directionMaterial;

    private void Start()
    {
        mainCamera = Camera.main;
        outsideMaterial = highlightOutside.GetComponent<Renderer>().material;
        directionMaterial = highlightDirection.GetComponent<Renderer>().material;
    }

    private void Update()
    {
        CheckForBlock();
    }

    private void CheckForBlock()
    {
        var matList = new List<Material>();
        float distance = 1f;
        if (Physics.Raycast(new Ray(transform.position, transform.position - mainCamera.transform.position),
                out RaycastHit hit, distance,
                layerMask))
        {
            matList.Add(highlightBlockedMat);
            highlightDirection.GetComponent<Renderer>().SetMaterials(matList);
            return;
        };
        matList.Add(directionMaterial);
        highlightDirection.GetComponent<Renderer>().SetMaterials(matList);
    }

    public void Cuttable()
    {
        outsideMaterial.color = Color.green;
        directionMaterial.color = Color.green;
    }

    public void NotCuttable()
    {
        outsideMaterial.color = Color.white;
        directionMaterial.color = Color.white;
    }

    private void OnDestroy()
    {
        Destroy(outsideMaterial);
        Destroy(directionMaterial);
    }
}
