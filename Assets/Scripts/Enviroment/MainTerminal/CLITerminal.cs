using UnityEngine;

public class CLITerminal : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject CLI;
    public void Interact()
    {
        Debug.Log("CLI Opened");
        CLI.SetActive(true);
    }
}
