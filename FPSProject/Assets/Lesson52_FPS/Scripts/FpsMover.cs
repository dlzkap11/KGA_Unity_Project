using UnityEngine;

/// <summary>
/// [52차시 2교시] 1인칭 이동 — CharacterController판.
///
/// 계약 두 줄:
///   ① 이동은 몸 기준  → transform.right / transform.forward (몸통이 돌면 "앞"도 같이 돈다 — 3교시에서 회수)
///   ② 중력은 셀프     → Rigidbody를 버렸으니 물리의 책임도 우리 것 (안 넣으면 허공을 수평으로 걷는다)
///
/// Rigidbody = 파도에 뜬 튜브(물리가 주도) / CharacterController = 레일 위의 카트(코드가 주도).
/// FPS는 폭발에 떠밀려 조준이 흔들리면 안 되므로 카트를 탄다. Rigidbody는 붙이지 않는다!
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class FpsMover : MonoBehaviour
{
    [Header("이동 설정")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float gravity = -20f;    // 음수! (아래 방향 가속도)

    [Header("점프 설정 (실습 레벨 3)")]
    [SerializeField] private float jumpHeight = 1.2f; // 점프 최고 높이(미터)

    private CharacterController controller;
    private Vector3 velocity; // 세로 속도의 "누적" 저장소 — 중력은 속도가 아니라 가속도이므로 누적이 필요

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        // ── 이동 (계약 ①) ──────────────────────────────────────────
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // 월드 축(Vector3.forward)이 아니라 "내가 지금 바라보는 방향" 기준 —
        // 3교시 MouseLook이 몸통을 돌리면 이 두 벡터도 같이 돌아서, 보는 방향이 곧 가는 방향이 된다.
        // Vector3.forward로 쓰면 몸이 돌아도 '앞'이 고정되는 계약 ① 위반!
        Vector3 move = transform.right * h + transform.forward * v;

        // ── 중력 (계약 ②) ──────────────────────────────────────────
        // 땅에 닿아 있고 + "떨어지는 중"일 때만 세로 속도 리셋
        // (점프로 막 올라가는 중(velocity.y > 0)에는 건드리면 안 되므로 조건 두 개)
        if (controller.isGrounded && velocity.y < 0)
        {
            // 0이 아니라 -2인 이유: CharacterController는 매 프레임 스윕 검사 방식이라
            // 세로 속도가 정확히 0이면 계단·경사의 미세 굴곡에서 isGrounded가 프레임마다 파닥거린다.
            // 살짝 음수를 유지해 "항상 바닥을 누르는 힘"을 준다.
            velocity.y = -2f;
        }

        // [레벨 3] 점프 — 접지 리셋보다 "뒤에" 있어야 한다!
        // 앞에 두면 바로 위의 리셋(-2)이 점프 속도를 덮어써서 "점프가 안 돼요"가 된다 (순서가 학습 지점)
        if (controller.isGrounded && Input.GetButtonDown("Jump"))
        {
            // v = √(h × -2g): 목표 높이에 딱 도달하는 초기 속도 (등가속도 공식에서 유도)
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // 중력은 가속도 → 매 프레임 속도에 누적 (deltaTime 곱해 프레임 독립)
        velocity.y += gravity * Time.deltaTime;

        // 수평+수직을 한 벡터로 합친 뒤 Time.deltaTime은 마지막에 "딱 한 번만"
        // (각각 곱하면 이중 적용되어 기어가고, 안 곱하면 순간이동)
        controller.Move((move * speed + velocity) * Time.deltaTime);
    }
}
