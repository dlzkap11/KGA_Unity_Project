using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// [51차시 데모 씬용] 플레이어 이동 — 마우스 "오른쪽" 클릭으로 이동.
///
/// ※ 왼쪽 클릭은 PlayerShooter의 사격에 쓰므로, 데모 씬에서는 RTS 관례대로
///   좌클릭=사격 / 우클릭=이동 으로 분리했다.
/// ※ 48차시 클릭 이동과 같은 파이프라인. 49·50차시 패키지의 클릭 이동과
///   클래스 이름만 다르다 (한 프로젝트에 함께 임포트해도 충돌 없도록).
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
public class ClickMoveController : MonoBehaviour
{
    [Header("클릭 판정 설정")]
    [SerializeField] private LayerMask groundMask;        // 바닥 레이어만 클릭 대상
    [SerializeField] private float maxRayDistance = 200f;

    private NavMeshAgent agent;
    private Camera mainCamera;
    private Health health; // 죽으면 조작이 스스로 꺼지기 위한 참조

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        mainCamera = Camera.main;
        health = GetComponent<Health>();
    }

    private void OnEnable()
    {
        // Player가 죽으면 조작도 스스로 꺼진다 (뒷정리 — 실습 레벨 1: "Player 죽었는데 계속 조작" 방지)
        if (health != null)
        {
            health.OnDied += HandleDied;
        }
    }

    private void OnDisable()
    {
        if (health != null)
        {
            health.OnDied -= HandleDied; // += / -= 는 반드시 짝
        }
    }

    private void HandleDied()
    {
        agent.isStopped = true; // 가던 길도 멈추고
        enabled = false;        // 조작 정지
    }

    private void Update()
    {
        // 오른쪽 버튼(1)을 누른 "그 순간"에만 목적지 갱신
        if (!Input.GetMouseButtonDown(1))
        {
            return;
        }

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, maxRayDistance, groundMask))
        {
            agent.SetDestination(hit.point);
        }
    }
}
