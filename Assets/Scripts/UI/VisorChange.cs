using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VisorChange : MonoBehaviour
{
    [Header("Visor Sprites")]
    [SerializeField] Sprite defaultVisor;
    [SerializeField] Sprite mildlyDamagedVisor;
    [SerializeField] Sprite badlyDamagedVisor;
    [SerializeField] Sprite hackingVisor;

    public Visor currentDamageState;

    public enum Visor
    {
        Default,
        MildlyDamaged,
        BadlyDamaged,
        Hacking
    }

    private void Awake()
    {
        currentDamageState = Visor.Default;
    }

    private void OnEnable()
    {
        PlayerHealth.OnHealthChanged += OnHealthChanged;
        GameManager.OnGameStateChanged += OnGameStateChanged;
    }

    private void OnDisable()
    {
        PlayerHealth.OnHealthChanged -= OnHealthChanged;
        GameManager.OnGameStateChanged -= OnGameStateChanged;
    }

    private void OnGameStateChanged(GameState newState)
    {
        if (newState == GameState.Hacking)
        {
            UpdateVisor(Visor.Hacking);
        }

        if (newState == GameState.Gameplay)
        {
            UpdateVisor(currentDamageState);
        }
    }

    private void OnHealthChanged(float health, float maxHealth)
    {
        if (health <= maxHealth * 0.25f)
        {
            UpdateVisor(Visor.BadlyDamaged);
        }
        else if (health <= maxHealth * 0.50f)
        {
            UpdateVisor(Visor.MildlyDamaged);
        }
        else
        {
            UpdateVisor(Visor.Default);
        }
    }

    private void UpdateVisor(Visor visor)
    {
        switch(visor)
        {
            case Visor.Default:
                gameObject.GetComponent<Image>().sprite = defaultVisor;
                currentDamageState = Visor.Default;
                break;
            case Visor.MildlyDamaged:
                gameObject.GetComponent<Image>().sprite = mildlyDamagedVisor;
                currentDamageState = Visor.MildlyDamaged;
                break;
            case Visor.BadlyDamaged:
                gameObject.GetComponent<Image>().sprite = badlyDamagedVisor;
                currentDamageState = Visor.BadlyDamaged;
                break;
            case Visor.Hacking:
                gameObject.GetComponent<Image>().sprite = hackingVisor;
                gameObject.GetComponent<Canvas>().sortingOrder = 1;
                break;
        }
    }

    private void Update()
    {
        if (gameObject.GetComponent<Canvas>().sortingOrder != -1 && gameObject.GetComponent<Image>().sprite != hackingVisor)
        {
            gameObject.GetComponent<Canvas>().sortingOrder = -1;
        }
    }
}
