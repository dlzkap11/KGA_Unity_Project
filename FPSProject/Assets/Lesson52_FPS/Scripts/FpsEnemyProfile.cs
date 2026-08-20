using UnityEngine;

/// <summary>
/// [52차시 5교시] AI 성격표 — 로직이 전혀 없는 순수 데이터 컨테이너(ScriptableObject).
///
/// MonoBehaviour는 씬의 오브젝트에 붙어야 존재하지만, ScriptableObject는 프로젝트 창의
/// "에셋 파일"로 독립적으로 존재한다. Enemy 프리팹이 몇 개든 같은 Profile 에셋 하나를
/// 다 같이 참조한다 — 데이터(수치)와 로직(두뇌·센서)의 완전한 분리 (32차시 아이템 데이터와 같은 해법).
///
/// ★ 성적표: 새 성격(탱커형)을 추가할 때 고치는 코드가 "0곳"이어야 한다 —
///   어제(51차시)의 "Health 0줄"과 같은 척도. 에셋 하나 추가로 끝나야 정상.
///
/// ⚠ 공유 참조이므로 런타임에 필드를 직접 수정하면 이 Profile을 쓰는 모든 적에 영향이 간다
///   — "읽기 전용 설정값"으로만 쓸 것.
/// </summary>
[CreateAssetMenu(menuName = "AI/FPS Enemy Profile")] // 우클릭 Create 메뉴에 등록 — 스크립트를 안 열고도 성격표를 계속 찍어낸다
public class FpsEnemyProfile : ScriptableObject
{
    [Header("Perception")] // Header는 로직에 영향 없음 — Inspector 구획 표시용
    public float sightRange = 18f;    // 감지 최대 거리 — 종이 설계의 "발견 별표" 거리와 맞춰볼 것
    public float fieldOfView = 110f;  // 감지 시야각(부채꼴 전체 각도) — 카메라 Field of View(렌더링 화각)와는 완전히 다른 값!

    [Header("Movement")]
    public float moveSpeed = 3.5f;    // 평상시(순찰) 속도
    public float chaseSpeed = 5f;     // 추적 속도 — Rusher 6 / Shooter 3.5, 성격 차이가 가장 크게 드러나는 값

    [Header("Combat")]
    public int damage = 10;           // 공격 피해량 (사격전은 53차시에서 사용)
    public float attackRange = 12f;   // 공격(사격) 가능 거리 — Gizmo 빨강 원의 반지름
    public float fireCooldown = 1.2f; // 공격 사이 대기(초) — 53차시 사격 쿨다운

    [Header("Survivability")]
    public int maxHealth = 100;       // 최대 체력 — 탱커형은 300 (51차시 Health와 결합 시 사용)
}
