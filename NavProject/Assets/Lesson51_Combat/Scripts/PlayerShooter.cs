using UnityEngine;

/// <summary>
/// [51차시 2교시] 히트스캔 사격 — 광선이 곧 총알 (레이저 포인터로 찍는 순간 명중 결정).
///
/// 구조는 48차시 클릭 이동과 같다: 클릭 감지 → 카메라에서 광선 → 검사 → 행동.
/// 마지막 "행동"만 SetDestination에서 TakeDamage로 바뀌었을 뿐이다.
///
/// ★ 오늘의 마법 한 줄: GetComponent는 인터페이스도 찾는다.
///   덕분에 "이게 바닥인지 적인지" 구분하는 if/else가 코드 어디에도 없다 —
///   이 스크립트는 자신이 쏜 대상의 구체적 타입을 전혀 모른다.
/// </summary>
public class PlayerShooter : MonoBehaviour
{
    [Header("사격 설정")]
    [SerializeField] private Camera mainCamera;      // 광선의 기준 카메라 (비워두면 Awake에서 Camera.main 캐싱)
    [SerializeField] private int damage = 10;        // 한 발당 피해량 — Enemy(30)를 3발에 처치
    [SerializeField] private float maxDistance = 100f;
    [SerializeField] private LayerMask hitMask = ~0; // 사격 판정 대상 (데모 씬에서는 Player 자신의 레이어만 제외)

    private Health health; // 내 체력 — 죽으면 사격이 스스로 꺼지기 위한 참조 (뒷정리)

    private void Awake()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        health = GetComponent<Health>();
    }

    private void OnEnable()
    {
        // Player가 죽으면 사격도 스스로 꺼진다 — Enemy 뒷정리와 똑같은 구독 패턴 (실습 레벨 1)
        if (health != null)
        {
            health.OnDied += HandleDied;
        }
    }

    private void OnDisable()
    {
        // += 를 쓰는 손이 -= 를 같이 쓰는 습관 — 반드시 짝으로 (4교시 구독 해제 유령)
        if (health != null)
        {
            health.OnDied -= HandleDied;
        }
    }

    private void HandleDied()
    {
        enabled = false; // 죽으면 사격 정지
    }

    private void Update()
    {
        // Down = 누른 "그 프레임만" (연사가 아니라 단발 — 연사·쿨다운은 FPS 차시에서)
        if (!Input.GetMouseButtonDown(0))
        {
            return;
        }

        // 48차시 클릭 이동과 완전히 같은 광선
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        
        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
        {
            Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.red, 1f);

            // ★ 마법 한 줄: 제네릭 자리에 인터페이스를 넣어도 동작한다.
            //   IDamageable을 구현한 컴포넌트가 있으면 그 인스턴스를, 없으면 null을 돌려준다
            IDamageable damageable = hit.collider.GetComponent<IDamageable>();
            Debug.Log(hit.collider.name + "피격!");
            // 이 검사 하나로 바닥·벽을 쐈을 때 조용히 무시된다
            if (damageable != null)
            {
                //Debug.Log(hit.collider.name + "피격!");
                damageable.TakeDamage(damage, gameObject);
            }
        }
    }
}
