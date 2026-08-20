using UnityEngine;

/// <summary>
/// [52차시 3교시] 마우스 룩 — Yaw(몸통)/Pitch(고개) 분리.
///
/// 분리 원칙: 사람도 뒤를 볼 땐 몸을 돌리고, 하늘을 볼 땐 고개만 든다.
///   좌우 = 몸통(playerBody)이 돈다 → 2교시의 "앞"(transform.forward)도 같이 돈다
///   상하 = 고개(카메라)만 돈다 — 단, 관절 한계(±80도)가 있다
///
/// 부착 위치: Player의 자식 카메라(눈높이 1.6~1.7). playerBody에는 부모 Player 연결.
/// 회전을 두 Transform으로 나누는 이유 = 짐벌락 회피 (한 Transform에서 세 축을 오일러로
/// 통제하면 ±90도 근처에서 축이 겹친다 — 그래서 한계도 90이 아니라 80).
/// </summary>
public class MouseLook : MonoBehaviour
{
    [Header("회전 설정")]
    [SerializeField] private Transform playerBody;   // 좌우 회전을 받을 몸통(부모 Player)
    [SerializeField] private float sensitivity = 5f; // 감도 — 2/5/10 체험 후 자기 값 (취향)
    [SerializeField] private float pitchLimit = 80f; // 고개 관절 한계. 90은 짐벌락을 스친다!

    private float pitch; // 고개 각도의 "누적값" — 매 프레임 통째로 다시 지정하기 위한 저장소

    private void Start()
    {
        // 커서를 화면 중앙에 잠근다 — 시선 돌리다 창 밖 클릭 방지.
        // "상태" 값이라 Update에 있을 이유가 없다 (에디터에서는 Esc로 잠금 해제)
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        // GetAxis: GetAxisRaw와 달리 스무딩이 들어가 각도가 뚝뚝 끊기지 않는다
        float mouseX = Input.GetAxis("Mouse X") * sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

        // [상하] 부호가 뺄셈인 이유: 마우스를 "위로" 올리면 Mouse Y는 양수인데,
        // 카메라가 "위를 보게" 하려면 로컬 X축 회전각은 작아지는(음의) 방향이어야 한다.
        // 더하기로 쓰면 "위를 보려는데 땅을 보는" 상하 반전 — FPS 입문자의 8할이 걸리는 그 저주
        pitch -= mouseY;

        // 관절 한계 — 없으면 고개가 한 바퀴 도는 공포 영화가 된다
        pitch = Mathf.Clamp(pitch, -pitchLimit, pitchLimit);

        // Rotate()로 누적하지 않고 매 프레임 "통째로 새로 지정" —
        // Y·Z가 항상 0으로 고정되어 롤(기울어짐)이 안 생긴다 (누적 방식은 축이 서서히 오염됨)
        transform.localRotation = Quaternion.Euler(pitch, 0f, 0f);

        // [좌우] 몸통은 관절 한계가 없으므로 상대 회전("이만큼 더 돌아라")으로 계속 누적해도 된다
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
