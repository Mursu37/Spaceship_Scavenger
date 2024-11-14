using UnityEngine;

public class ChangeColor : MonoBehaviour
{
    
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        
        spriteRenderer = GetComponent<SpriteRenderer>();

        
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.yellow;
        }
    }
}
