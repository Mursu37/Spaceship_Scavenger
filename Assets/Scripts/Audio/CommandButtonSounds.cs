using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandButtonSounds : MonoBehaviour
{
    public void Click()
    {
        AudioManager.PlayAudio("TerminalSelect", 1, 1, false, null, true);
    }
}
