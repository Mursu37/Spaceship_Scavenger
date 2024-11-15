using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanisterDetonation : MonoBehaviour, IInteractable
{
    private Explosives explosives;
    private CanisterAudio canisterAudio;
    private bool canDetonate = true;
    [SerializeField] private ParticleSystem steam;

    // Start is called before the first frame update
    void Start()
    {
        explosives = GetComponent<Explosives>();
    }

    public void Interact()
    {
        Debug.Log("Detonate");
        StartCoroutine(Detonate());
    }

    private IEnumerator Detonate()
    {
        while (true)
        {
            canisterAudio = FindObjectOfType<CanisterAudio>();
            canisterAudio.PlayFuzeSound(gameObject);
            steam.Play();
            yield return new WaitForSeconds(4f);
            explosives.Explode();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.relativeVelocity.magnitude >= 3f && canDetonate)
        {
            canDetonate = false;
            StartCoroutine(Detonate());
        }
    }
}
