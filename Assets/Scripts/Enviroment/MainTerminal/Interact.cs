using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour
{

    [SerializeField] private LayerMask interactableLayerMask;
    private Camera camera;
    
    private void Start()
    {
        camera = Camera.main;
    }

    void Update()
    {
        if (Input.GetButtonDown("Interact"))
        {
            if (Physics.Raycast(camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)), out RaycastHit hit, 2f,
                    interactableLayerMask))
            {
                if (hit.transform.CompareTag("RepairKit"))
                {
                    transform.GetComponent<IHealth>().Heal(5);
                }
                if (hit.transform.GetComponent<IInteractable>() != null)
                {
                    hit.transform.GetComponent<IInteractable>().Interact();
                }
            }
        }
    }
}
