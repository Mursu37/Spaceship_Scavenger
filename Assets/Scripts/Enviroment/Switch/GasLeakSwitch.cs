using System.Collections;
using UnityEngine;

public class GasLeakSwitch : MonoBehaviour, IInteractable
{
    private Animator animator;
    private ParticleSystem gasParticle;
    private GasTrigger gasTrigger;

    [SerializeField] private GameObject gasLeak;
    [SerializeField] private GameObject[] gasLeaks;

    private void Start()
    {
        animator = GetComponent<Animator>();
        if (gasLeak != null)
        {
            gasTrigger = gasLeak.GetComponent<GasTrigger>();
            gasParticle = gasLeak.transform.GetChild(0).GetComponent<ParticleSystem>();
        }
    }

    public void Interact()
    {
        Debug.Log("Gas leak off.");
        animator.Play("TurnOn");
        StartCoroutine(TurnOffGas());
    }

    private IEnumerator TurnOffGas()
    {
        while (true)
        {
            yield return new WaitForSeconds(2f);
            gasTrigger.enabled = false;
            gasParticle.Stop();
        }
    }
}
