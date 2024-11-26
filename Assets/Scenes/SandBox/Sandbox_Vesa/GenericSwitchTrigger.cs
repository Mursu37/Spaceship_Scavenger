using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericSwitchTrigger : MonoBehaviour
{
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    void SwitchTriggered()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
