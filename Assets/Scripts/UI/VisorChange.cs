using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VisorChange : MonoBehaviour
{
    public static VisorChange instance;

    [SerializeField] GameObject visorObject;

    [Header("Visor Sprites")]
    [SerializeField] Sprite defaultVisor;
    [SerializeField] Sprite mildlyDamagedVisor;
    [SerializeField] Sprite badlyDamagedVisor;
    [SerializeField] Sprite hackingVisor;

    public static Visor currentDamageState = Visor.Default;

    public enum Visor
    {
        Default,
        MildlyDamaged,
        BadlyDamaged,
        Hacking
    }

    private void Awake()
    {
        instance = this;
        currentDamageState = Visor.Default;
    }

    public static void UpdateVisor(Visor visor)
    {
        switch(visor)
        {
            case Visor.Default:
                instance.visorObject.GetComponent<Image>().sprite = instance.defaultVisor;
                currentDamageState = Visor.Default;
                break;
            case Visor.MildlyDamaged:
                instance.visorObject.GetComponent<Image>().sprite = instance.mildlyDamagedVisor;
                currentDamageState = Visor.MildlyDamaged;
                break;
            case Visor.BadlyDamaged:
                instance.visorObject.GetComponent<Image>().sprite = instance.badlyDamagedVisor;
                currentDamageState = Visor.BadlyDamaged;
                break;
            case Visor.Hacking:
                instance.visorObject.GetComponent<Image>().sprite = instance.hackingVisor;
                instance.visorObject.GetComponent<Canvas>().sortingOrder = 1;
                break;
        }
    }

    private void Update()
    {
        if (instance.visorObject.GetComponent<Canvas>().sortingOrder != -1 && instance.visorObject.GetComponent<Image>().sprite != instance.hackingVisor)
        {
            instance.visorObject.GetComponent<Canvas>().sortingOrder = -1;
        }
    }
}
