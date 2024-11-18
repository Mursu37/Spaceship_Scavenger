using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TestEditorButtonDispatcher : MonoBehaviour
{
    public UnityEvent runtimeEvent;

    // Start is called before the first frame update
    public void DispatchEvent()
    {
        runtimeEvent.Invoke();
    }
}
