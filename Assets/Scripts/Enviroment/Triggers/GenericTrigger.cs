using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericTrigger : MonoBehaviour
{
    [SerializeField] private EventDispatcher dispatcher;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            dispatcher.TriggerEvent();
        }
    }
}
