using System.Collections;
using UnityEngine;

public class GasLeakSwitch : Switch
{
    private ParticleSystem gasParticle;
    private GasTrigger gasTrigger;

    [SerializeField] private GameObject gasLeaks;

    protected override IEnumerator SwitchAction()
    {
        while (true)
        {
            yield return new WaitForSeconds(2f);
            if (gasLeaks != null)
            {
                for (int i = 0; i < gasLeaks.transform.childCount; i++)
                {
                    GameObject leak = gasLeaks.transform.GetChild(i).gameObject;
                    if (leak != null)
                    {
                        gasTrigger = leak.GetComponent<GasTrigger>();
                        gasParticle = leak.transform.GetChild(0).GetComponent<ParticleSystem>();
                        if (gasTrigger != null && gasParticle != null)
                        {
                            gasParticle.Stop();
                            gasTrigger.enabled = false;
                        } 
                    }
                }
            }
        }
    }
}
