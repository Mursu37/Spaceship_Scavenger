using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

[ExecuteAlways]
public class ExplosionShaderDriver : MonoBehaviour
{
    private MeshRenderer meshRenderer;

    [SerializeField]
    private Camera cam;
    private Material material;

    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        material = meshRenderer.sharedMaterial;


    }

    // Update is called once per frame
    void Update()
    {
        Material _material = material;

        if (cam != null && material != null)
        {
            // c#
            Matrix4x4 V = cam.worldToCameraMatrix;
            Matrix4x4 P = GL.GetGPUProjectionMatrix(cam.projectionMatrix, false);
            Matrix4x4 VP = P * V;

            // Apply correction for the 90-degree rotation
            Matrix4x4 correction = Matrix4x4.Rotate(Quaternion.Euler(90, 0, 0));
            VP = correction * VP;

            _material.SetMatrix("_Custom_Matrix_VP", VP);
            meshRenderer.sharedMaterial = _material;

        }

        /*   if (explosionMaterial.HasProperty("_worldPosition"))
           {
               Debug.Log("changedworldPosition");
               Vector3 _worldPosition = transform.position;
               _material.SetVector("_startPoint", _worldPosition);
           }
        */
        meshRenderer.sharedMaterial = _material;
    }
}
