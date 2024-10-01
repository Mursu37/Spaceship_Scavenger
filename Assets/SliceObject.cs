using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;
using UnityEngine.InputSystem;


public class SliceObject : MonoBehaviour
{

    public Transform planeDebug;
    public GameObject target;
    public Material crossSectionMaterial;
    public float cutForce = 100;
    //public Transform cameraTransform;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Slice(target);
        }

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Slice(hit.transform.gameObject, hit.point); 
            } 
        } 
    }
    public void Slice(GameObject target, Vector3 hitPoint)
    {
        Vector3 sliceDirection = Vector3.up;
        EzySlice.Plane slicingPlane = new EzySlice.Plane(hitPoint, sliceDirection);
        SlicedHull hull = target.Slice(slicingPlane);
    }

    public void Slice(GameObject target)
    {
       // Defining the slicing plane based on the camera orientation for a more dynamic experience
       // Vector3 slicePosition = cameraTransform.position + cameraTransform.forward * 2.0f;
       // Vector3 sliceDirection = cameraTransform.up;
       // SlicedHull hull = target.Slice(slicePosition, sliceDirection);


        SlicedHull hull = target.Slice(planeDebug.position, planeDebug.up);

        if (hull != null)
        {
            GameObject upperHull = hull.CreateUpperHull(target, crossSectionMaterial);
            SetupSlicedComponent(upperHull);

            GameObject lowerHull = hull.CreateLowerHull(target, crossSectionMaterial);
            SetupSlicedComponent(lowerHull);

            

            Destroy(target);

            Rigidbody upperRigidbody = upperHull.AddComponent<Rigidbody>();
            Rigidbody lowerRigidbody = lowerHull.AddComponent<Rigidbody>();
            upperRigidbody.velocity = Vector3.zero;
            lowerRigidbody.velocity = Vector3.zero;
            upperRigidbody.angularVelocity = Vector3.zero;
            lowerRigidbody.angularVelocity = Vector3.zero;



        }
    }

    void SetPhysicsProperties(Rigidbody rb)
    { 
        rb.mass = 1.0f;
        rb.useGravity = true;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        StartCoroutine(FreezeMovementTemporarily(rb));
    }

    IEnumerator FreezeMovementTemporarily(Rigidbody rb)
    { 
        rb.isKinematic = true;
        yield return new WaitForSeconds(0.2f);
        rb.isKinematic = false;
    }
        public void SetupSlicedComponent(GameObject slicedObject)
    {
        Rigidbody rb = slicedObject.AddComponent<Rigidbody>();
        MeshCollider collider = slicedObject.AddComponent<MeshCollider>();
        collider.convex = true;
        rb.AddExplosionForce(cutForce * 0.1f, slicedObject.transform.position, 0.5f);

    }
}
