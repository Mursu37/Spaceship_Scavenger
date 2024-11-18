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
    [SerializeField]
    private VerticalLayoutGroup verticalLayoutGroup;
    private List<Selectable> selectablesList = new List<Selectable>();
    private Selectable lastListSelectable;

    // Start is called before the first frame update
    void Start()
    {
        inputField = this.GetComponent<TMP_InputField>();
        _eventsystem = FindFirstObjectByType<EventSystem>();
        AddListItems();
    }

    private void AddListItems()
    {
        selectablesList.AddRange(verticalLayoutGroup.gameObject.GetComponentsInChildren<Selectable>());
        SetSelectableOrder();
    }

    private void ClearListItems()
    {
        selectablesList.Clear();
    }

    private void SetSelectableOrder()
    {
        Debug.Log("Number of selectables: " + selectablesList.Count);

        if (selectablesList != null && selectablesList.Count > 0)
        {
            for (int i = 0; i < selectablesList.Count; i++)
            {
                Navigation navigation = selectablesList[i].navigation;
                navigation.mode = Navigation.Mode.Explicit;
                navigation.selectOnUp = null;
                navigation.selectOnDown = null;



                if (i == 0)
                {
                    navigation.selectOnDown = selectablesList[i + 1];
                }
                else if (i > 0 && i < selectablesList.Count - 1)
                {
                    navigation.selectOnUp = selectablesList[i - 1];
                    if (selectablesList[i + 1].interactable)
                    {
                        navigation.selectOnDown = selectablesList[i + 1];
                    }
                    else {
                        navigation.selectOnDown = inputField as Selectable;
                    }

                }
                else if (i == selectablesList.Count - 1)
                {
                    navigation.selectOnUp = selectablesList[i - 1];
                    lastListSelectable = selectablesList[i];
                    navigation.selectOnDown = inputField as Selectable;
                }

                selectablesList[i].navigation = navigation;
                
            }

            Navigation inputNavigation = inputField.navigation;
            inputNavigation.mode = Navigation.Mode.Explicit;
            inputNavigation.selectOnUp = selectablesList[selectablesList.Count - 1];
            inputNavigation.selectOnDown = null;
            inputField.navigation = inputNavigation;

        }

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

                if (cooldownTimer <= 0)
                {

                    if (Input.GetKeyDown(KeyCode.UpArrow))
                    {
                        if (EventSystem.current.currentSelectedGameObject == inputField.gameObject)
                        {
                            Debug.Log(lastListSelectable.gameObject.name);
                            EventSystem.current.SetSelectedGameObject(lastListSelectable.gameObject);
                            cooldownTimer = navigationCooldown; // Set cooldown
                        }

                    }
                }


        }
    }
}
