using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class Slicer : MonoBehaviour
{
    [SerializeField] private GameObject slicer;

    private void Start()
    {
        //slicer.transform.forward = Camera.main.transform.forward;
        //slicer.transform.up = Camera.main.transform.up;
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            var mesh = slicer.GetComponent<MeshFilter>().sharedMesh;
            var center = mesh.bounds.center;
            var extents = mesh.bounds.extents;

            extents = new Vector3(extents.x * slicer.transform.localScale.x,
                                  extents.y * slicer.transform.localScale.y,
                                  extents.z * slicer.transform.localScale.z);

            RaycastHit[] hits = Physics.BoxCastAll(slicer.transform.position, extents, slicer.transform.forward, slicer.transform.rotation, extents.z);

            foreach (RaycastHit hit in hits)
            {
                var obj = hit.collider.gameObject;
                var sliceObj = obj.GetComponent<Slice>();

                if (sliceObj != null)
                {
                    sliceObj.ComputeSlice(slicer.transform.up, slicer.transform.position);
                    gameObject.GetComponent<SliceManager>().UpdateList(/*sliceObj.GetComponent<Rigidbody>().isKinematic*/);
                }
            }
        }
    }
}
