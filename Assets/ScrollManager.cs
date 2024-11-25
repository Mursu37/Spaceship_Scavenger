using UnityEngine;
using UnityEngine.UI;

public class ScrollManager : MonoBehaviour
{
    [SerializeField] private GravityGun gravityGun; 
    [SerializeField] private Image scrollCommandImage; 

    private void Start()
    {
        
        if (scrollCommandImage != null)
        {
            scrollCommandImage.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        // Check if the GravityGun is grabbing an object
        if (gravityGun != null)
        {
            if (gravityGun.IsGrabbingValidObject()) 
            {
                // Show the scroll command UI
                if (scrollCommandImage != null)
                {
                    scrollCommandImage.gameObject.SetActive(true);
                }
            }
            else
            {
                // Hide the scroll command UI
                if (scrollCommandImage != null)
                {
                    scrollCommandImage.gameObject.SetActive(false);
                }
            }
        }
    }
}

