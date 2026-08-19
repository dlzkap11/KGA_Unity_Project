using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerInput))]
public class FPSMover : MonoBehaviour
{

    [SerializeField] private CharacterController controller;
    [SerializeField] private float gravity;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpPower;

    private Vector2 moveInput;
    [SerializeField] private Vector3 velocity;

    private void Awake()
    {
        if (controller == null)
            controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        //Vector3 moveVec = new Vector3(moveInput.x, 0f, moveInput.y).normalized;
        Vector3 moveVec = transform.right * moveInput.x + transform.forward * moveInput.y;

        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move((moveVec * moveSpeed + velocity) * Time.deltaTime);

    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }


    void OnJump(InputValue value)
    {
        if (controller.isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpPower * -2f * gravity);
        }
    }
}
