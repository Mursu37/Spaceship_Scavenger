using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddArrayToXray : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private GameObject[] targetsToXray;

    public void SetGroupToLayer(int newlayer)
    {
        for (int i = 0; i < targetsToXray.Length; i++)
        {
            if (targetsToXray[i] != null)
            {
                SetLayerRecursively(targetsToXray[i], newlayer);
            }
        }
    }

    public void SetLayerRecursively(GameObject obj, int newlayer)
    {
        obj.layer = newlayer;
        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, newlayer);
        }
    }

}
