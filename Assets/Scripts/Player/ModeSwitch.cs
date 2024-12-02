using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeSwitch : MonoBehaviour
{
    private GravityGun gravityGun;
    private Cutting cutting;
    private Slicer slicer;

    [SerializeField] private GameObject slicerObject;
    [SerializeField] private GameObject crosshair;
    [SerializeField] private GameObject cuttingCrosshairHorizontal;
    [SerializeField] private GameObject cuttingCrosshairVertical;
    [SerializeField] private SkinnedMeshRenderer[] meshRenderers;
    [SerializeField] private Material gravityMaterial;
    [SerializeField] private Material cuttingMaterial;
    public int selectedMode;

    // Start is called before the first frame update
    private void Start()
    {
        gravityGun = GetComponent<GravityGun>();
        cutting = GetComponent<Cutting>();
        slicer = GetComponent<Slicer>();
        selectedMode = 0;

    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0f && !PauseGame.isPaused)
        {
            selectedMode++;
            if (selectedMode > 1)
            {
                selectedMode = 0;
            }
            SelectMode();
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0f && !PauseGame.isPaused)
        {
            selectedMode--;
            if (selectedMode < 0)
            {
                selectedMode = 1;
            }
            SelectMode();
        }

        if (Input.GetButtonDown("Num1") && !PauseGame.isPaused)
        {
            selectedMode = 0;
            SelectMode();
        }

        if (Input.GetButtonDown("Num2") && !PauseGame.isPaused)
        {
            selectedMode = 1;
            SelectMode();
        }
    }

    private void SelectMode()
    {
        switch(selectedMode)
        {
            case 0:
                slicerObject.SetActive(false);
                gravityGun.enabled = true;
                cutting.enabled = false;
                slicer.enabled = false;
                crosshair.SetActive(true);
                cuttingCrosshairHorizontal.SetActive(false);
                cuttingCrosshairVertical.SetActive(false);

                if (meshRenderers != null && meshRenderers.Length > 0)
                {
                    Material material = meshRenderers[0].sharedMaterial;
                    material = gravityMaterial;
                    for (int i = 0; i < meshRenderers.Length; i++)
                    {
                        meshRenderers[i].sharedMaterial = material;
                    }
                }
                break;
            case 1:
                slicerObject.SetActive(true);
                gravityGun.enabled = false;
                cutting.enabled = true;
                slicer.enabled = true;
                crosshair.SetActive(false);
                cuttingCrosshairHorizontal.SetActive(true);
                cuttingCrosshairVertical.SetActive(false);

                if (meshRenderers != null && meshRenderers.Length > 0)
                {
                    Material material = meshRenderers[0].sharedMaterial;
                    material = cuttingMaterial;
                    for (int i = 0; i < meshRenderers.Length; i++)
                    {
                        meshRenderers[i].sharedMaterial = material;
                    }
                }
                break;
        }
    }
}
