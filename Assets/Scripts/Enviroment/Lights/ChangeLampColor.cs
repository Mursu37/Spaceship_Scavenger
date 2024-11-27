using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeLampColor : MonoBehaviour
{
    [SerializeField]
    private Light[] lights;
    [SerializeField]
    private Material material_1;
    [SerializeField]
    private Material material_2;
    [SerializeField]
    private MeshRenderer m_meshRenderer;

    [ColorUsage(true, true)]
    public Color color1;
    [ColorUsage(true, true)]
    public Color color2;




    // Start is called before the first frame update
    void Start()
    {
        if (m_meshRenderer == null)
        {
            m_meshRenderer = GetComponent<MeshRenderer>();
        }

    }

    // Update is called once per frame
    public void ChangeLightColor(int colorNumber)
    {
        Material material;
        Material[] materials;
        materials = m_meshRenderer.sharedMaterials;

        switch (colorNumber)
        {
            case 1:
                if (lights.Length > 0)
                {
                    for (int i = 0; i < lights.Length; i++)
                    {
                        if (lights[i] != null)
                        {
                            lights[i].color = color1;
                        }
                    }
                }
                material = material_1;
                materials[1] = material;
                m_meshRenderer.sharedMaterials = materials;
                break;
            case 2:
                if (lights.Length > 0)
                {
                    for (int i = 0; i < lights.Length; i++)
                    {
                        if (lights[i] != null)
                        {
                            lights[i].color = color2;
                        }
                    }
                }
                material = material_2;
                materials[1] = material;
                m_meshRenderer.sharedMaterials = materials;
                break;

            default:
                //
                break;
        }
    }
}
