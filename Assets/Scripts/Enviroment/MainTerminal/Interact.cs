using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Interact : MonoBehaviour
{

    [SerializeField] private LayerMask interactableLayerMask;
    [SerializeField] private GameObject interactionTextObject;
    private GameObject currentTextObject;
    private Camera camera;
    public GameObject currentlyHighlighted;
    
    private void Start()
    {
        camera = Camera.main;
    }

    void Update()
    {
        if (Input.GetButtonDown("Interact") && !PauseGame.isPaused)
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
        if (currentTextObject != null) Destroy(currentTextObject);
        if (Physics.Raycast(camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)), out RaycastHit hit2, 10f,
                interactableLayerMask))
        {
            currentlyHighlighted = hit2.transform.gameObject;
            if (interactionTextObject == null) return;
            Vector3 point = hit2.collider.bounds.center;
            point.y = point.y + hit2.collider.bounds.extents.y + 1f;
            currentTextObject = Instantiate(interactionTextObject, point, quaternion.identity);
        }
        else
        {
            currentlyHighlighted = null;
        }
    }
}
