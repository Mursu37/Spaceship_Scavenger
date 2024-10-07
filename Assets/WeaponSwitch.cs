using UnityEngine;

public class WeaponSwitch : MonoBehaviour
{
    public int selectedWeapon = 0;
    private GravityGun gravityGun;
    private SliceObject sliceObject;
    [SerializeField] private GameObject multitool;

    // Start is called before the first frame update
    void Start()
    {
        //SelectWeapon();
        gravityGun = GetComponent<GravityGun>();
        sliceObject = GetComponent<SliceObject>();

        gravityGun = multitool.GetComponent<GravityGun>();
        sliceObject = multitool.GetComponent<SliceObject>();
    }

    // Update is called once per frame
    void Update()
    {
        int previousSelectedWeapon = selectedWeapon;

        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            if (selectedWeapon >= transform.childCount - 1)
                selectedWeapon = 0;
            else
                selectedWeapon++;

        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            if (selectedWeapon <= 0)
                selectedWeapon = transform.childCount - 1;
            else
                selectedWeapon--;

        }

        if (previousSelectedWeapon != selectedWeapon)

        {
            SelectWeapon();
        }

        if (selectedWeapon == 0)
        {
            gravityGun.enabled = true;
            sliceObject.enabled = false;
        }
        else if (selectedWeapon == 1)
        {
            gravityGun.enabled = false;
            sliceObject.enabled = true;
        }
    }

    void SelectWeapon()
    {
        int i = 0;
        foreach (Transform weapon in transform)
        {
            if (i == selectedWeapon)
                weapon.gameObject.SetActive(true);
            else
                weapon.gameObject.SetActive(false);

            i++;



        }
    }
}
