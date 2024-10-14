using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Mode : MonoBehaviour
{
    private WeaponSwitch weaponSwitch;
    private TextMeshProUGUI modeText;
    [SerializeField] private GameObject weaponHolder;

    // Start is called before the first frame update
    void Start()
    {
        if (weaponHolder != null)
        {
            weaponSwitch = weaponHolder.GetComponent<WeaponSwitch>();
        }

        modeText = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (weaponSwitch.selectedWeapon == 0)
        {
            modeText.text = "Mode: Grappling";
        }
        else if (weaponSwitch.selectedWeapon == 1)
        {
            modeText.text = "Mode: Cutting";
        }
    }
}
