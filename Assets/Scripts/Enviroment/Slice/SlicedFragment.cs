using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SlicedFragment : MonoBehaviour
{
    private Rigidbody rb;
    private SliceManager sliceManager;
    private GameObject multitool;

    public bool canCollide = false;

    // Start is called before the first frame update
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddExplosionForce(0.5f, Camera.main.transform.position, 100f, 0f, ForceMode.VelocityChange);

        multitool = GameObject.Find("Multitool");
        if (multitool != null )
        {
            sliceManager = multitool.GetComponent<SliceManager>();
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("SliceHold"))
        {
            rb.isKinematic = true;
        }
    }
}
