using System.Collections.Generic;
using UnityEngine;

public class CuttingLine : MonoBehaviour
{
    private int newLayerMask = 1 << 9;

    private void OnTriggerEnter(Collider other)
    {
        MeshRenderer renderer = other.gameObject.GetComponent<MeshRenderer>();

        if (renderer != null)
        {
            // Add the new layer mask to the collideing object
            renderer.renderingLayerMask |= (uint)newLayerMask;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        MeshRenderer renderer = other.gameObject.GetComponent<MeshRenderer>();

        if (renderer != null)
        {
            // Remove the new layer mask from the object
            renderer.renderingLayerMask &= (uint)~newLayerMask;
        }
    }
}
