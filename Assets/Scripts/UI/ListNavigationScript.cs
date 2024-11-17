using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class ListNavigationScript : MonoBehaviour
{
    private EventSystem _eventsystem;
    private TMP_InputField inputField;
    [SerializeField]
    private float navigationCooldown = 0.2f;
    private float cooldownTimer = 0f;
    // Start is called before the first frame update
    void Start()
    {
        inputField = this.GetComponent<TMP_InputField>();
        _eventsystem = FindFirstObjectByType<EventSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.unscaledDeltaTime;
        }

        if (inputField.isFocused)
        {
            GameObject currentselected = EventSystem.current.currentSelectedGameObject;
            if (currentselected == inputField.gameObject)
            {
                Selectable current = currentselected.GetComponent<Selectable>();
                Selectable next = current.FindSelectableOnDown();
                Selectable previous = current.FindSelectableOnUp();

                if (cooldownTimer <= 0)
                {



                    if (Input.GetAxisRaw("Vertical") > 0.1 && !Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
                    {
                        if (previous != null)
                        {
                            previous.Select();
                            cooldownTimer = navigationCooldown; // Set cooldown
                        }

                    }
                    else if (Input.GetAxisRaw("Vertical") < -0.1 && !Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
                    {
                        if (next != null)
                        {
                            next.Select();
                            cooldownTimer = navigationCooldown; // Set cooldown
                        }
                    }
                }

            }
        }
    }
}
