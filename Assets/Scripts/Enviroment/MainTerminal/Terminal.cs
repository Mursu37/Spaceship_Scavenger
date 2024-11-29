using CLI.FSM;
using UnityEngine;
using UnityEngine.Rendering;

namespace Enviroment.MainTerminal
{
    public class Terminal : MonoBehaviour, IInteractable
    {
        [SerializeField] private StateController stateController;
        [SerializeField] private GameObject CLI;
        
        public void Interact()
        {
            Debug.Log(true);
            CLI.SetActive(true);
            stateController.enabled = true;
            AudioManager.PlayAudio("InteractBeep2", 1, 1, false, null, true);
        }
    }
}
