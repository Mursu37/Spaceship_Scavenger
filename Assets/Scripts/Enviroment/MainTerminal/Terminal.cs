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
            CLI.SetActive(true);
            stateController.enabled = true;
            AudioManager.PlayAudio("InteractBeep2", 0.5f, 1, false, null, true);
        }
    }
}
