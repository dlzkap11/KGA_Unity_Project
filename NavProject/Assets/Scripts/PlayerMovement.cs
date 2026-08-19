using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float gravity = -20f;

    private CharacterController controller;
    [SerializeField] private Vector3 verticalVelocity;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        // 기본적으로 Horizontal은 A/D, Vertical은 W/S에 연결되어 있습니다.
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 moveDirection = new Vector3(horizontal, 0f, vertical);

        // 대각선 이동 시 속도가 빨라지는 문제 방지
        moveDirection = Vector3.ClampMagnitude(moveDirection, 1f);

        // 플레이어의 로컬 방향 기준으로 이동
        Vector3 movement = transform.TransformDirection(moveDirection);
        controller.Move(movement * moveSpeed * Time.deltaTime);

        // 지면에 있을 때 아래로 계속 떨어지는 것을 방지
        if (controller.isGrounded && verticalVelocity.y < 0f)
        {
            verticalVelocity.y = -2f;
        }

        // 중력 적용
        verticalVelocity.y += gravity * Time.deltaTime;
        controller.Move(verticalVelocity * Time.deltaTime);
    }
}