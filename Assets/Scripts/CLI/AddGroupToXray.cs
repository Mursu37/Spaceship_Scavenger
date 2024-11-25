using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddGroupToXray : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    private GameObject targetGroup;


    public void SetGroupToLayer(int newlayer)
    {
        SetLayerRecursively(targetGroup, newlayer);
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
