using UnityEngine;

public class CLITerminal : MonoBehaviour, IInteractable
{
    [SerializeField] private Canvas CLI;
    public void Interact()
    {
        Debug.Log("CLI Opened");
        CLI.gameObject.SetActive(true);
    }
}
