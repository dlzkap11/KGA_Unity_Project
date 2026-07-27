using UnityEngine;

public class PlayerMove : MonoBehaviour
{




    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Color hitColor = Color.red;

    private Color originColor;

    void Awake()
    {
        originColor = spriteRenderer.color;
    }

    
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        if (horizontal != 0)
            spriteRenderer.flipX = horizontal < 0;

        if (Input.GetKeyDown(KeyCode.H))
            spriteRenderer.color = hitColor;

        if (Input.GetKeyDown(KeyCode.R))
            spriteRenderer.color = originColor;
    }
}
