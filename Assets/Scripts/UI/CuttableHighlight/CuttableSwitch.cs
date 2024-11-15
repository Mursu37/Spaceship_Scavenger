using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttableSwitch : MonoBehaviour
{
    [SerializeField] private GameObject highlightOutside;
    [SerializeField] private GameObject highlightDirection;

    private Material outsideMaterial;
    private Material directionMaterial;

    private void Start()
    {
        outsideMaterial = highlightOutside.GetComponent<Renderer>().material;
        directionMaterial = highlightDirection.GetComponent<Renderer>().material;
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
