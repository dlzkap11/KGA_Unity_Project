using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// [52차시] Profile 기반 적 두뇌 — 순찰 / 발견(시야 3중 판정) / 추적 / 공격 거리 정지.
///
/// ★ 5교시 리팩터링의 완성형: 감지 거리·속도·공격 거리 같은 "수치"가 코드에 단 하나도 없다.
///   전부 profile(ScriptableObject 성격표)에서 읽는다 — 같은 프리팹이 Profile만 바꿔 끼우면
///   돌격병(Rusher)도 저격수(Shooter)도 탱커(Tank)도 된다. 새 성격 추가 = 코드 수정 0곳.
///
/// ⚠ "Inspector엔 20인데 적은 12에서 반응한다"면 → 어딘가에 옛 하드코딩 숫자가 남은 것.
///   리팩터링은 전수 교체다 — 전체 검색(Ctrl+Shift+F)으로 옛 숫자를 다 뒤질 것 (5교시 진단).
///
/// 사격(실제 공격)은 53차시에서 — 오늘은 공격 거리에서 멈춰 노려보는 것까지.
/// 51차시 Health와 결합하려면: 판정 맨 위에 IsDead 가드 한 줄 (README 참고).
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
public class FpsEnemyBrain : MonoBehaviour
{
    [Header("성격표 (5교시 — 수치는 전부 여기서)")]
    [SerializeField] private FpsEnemyProfile profile; // Rusher/Shooter/Tank 에셋을 갈아 끼운다

    [Header("감지 대상")]
    [SerializeField] private Transform target;        // 비워두면 Awake에서 Player 태그 자동 검색
    [SerializeField] private LayerMask obstacleMask;  // 시선을 가리는 레이어(벽만! — 49차시 그대로)
    [SerializeField] private float eyeHeight = 1.5f;  // 눈높이 (발밑 광선은 바닥에 걸린다)

    [Header("순찰 설정")]
    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private float patrolWaitTime = 2f;
    [SerializeField] private float arrivalThreshold = 0.5f;

    private NavMeshAgent navAgent;
    private Health health; // 51차시 부품 — 죽음 판정용 (없으면 무시되는 선택적 연결)
    private bool hasDetectedTarget; // "봤다"는 기억 (49차시 FSM의 최소 단위)
    private int patrolIndex;
    private float waitTimer;

    private void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();
        health = GetComponent<Health>();
    }

    private void Update()
    {
        // ★ 판정 맨 위: 죽음이 모든 판단을 이긴다 (50차시 설계표 "Die는 맨 위" — 실습 레벨 1 연결)
        //   이 가드가 없으면 Die 모션이 나온 뒤에도 시체가 계속 쫓아온다
        if (health != null && health.IsDead)
        {
            navAgent.isStopped = true;
            return;
        }

        // 가드 절: 성격표나 타깃이 없으면 판단 불가
        if (profile == null || target == null)
        {
            return;
        }

        float distance = Vector3.Distance(transform.position, target.position);

        // [발견] 아직 못 봤고 + 시야 3중 판정(거리·시야각·벽 가림) 통과 → 기억
        if (!hasDetectedTarget && CanSeeTarget(distance))
        {
            hasDetectedTarget = true;
            Debug.Log($"[FpsEnemyBrain:{name}] 플레이어 발견! (거리 {distance:F1}m — 종이 설계의 별표와 비교해볼 것)");
        }

        if (!hasDetectedTarget)
        {
            Patrol();
            return;
        }

        // ── 발견 후 ──
        if (distance <= profile.attackRange)
        {
            // [공격 자리] 사거리 안 — 멈춰서 노려본다 (실제 사격은 53차시)
            navAgent.isStopped = true;
            LookAtTarget();
        }
        else
        {
            // [추적] 성격표의 추적 속도로 — Rusher(6)는 무섭게, Shooter(3.5)는 천천히 조여온다
            navAgent.isStopped = false;
            navAgent.speed = profile.chaseSpeed;
            navAgent.SetDestination(target.position);
        }
    }

    /// <summary>시야 3중 판정 (49차시 그대로) — 수치만 profile에서.</summary>
    private bool CanSeeTarget(float distance)
    {
        // [거리]
        if (distance > profile.sightRange)
        {
            return false;
        }

        // [시야각] 단위 벡터 내적 = cosθ, 기준값은 시야각 절반 (Deg2Rad 잊지 말 것!)
        Vector3 dirToTarget = (target.position - transform.position).normalized;
        float cosHalfFov = Mathf.Cos(profile.fieldOfView * 0.5f * Mathf.Deg2Rad);

        if (Vector3.Dot(transform.forward, dirToTarget) < cosHalfFov)
        {
            return false;
        }

        // [가림] 눈높이 광선이 벽에 막히면 안 보임 —
        // 허리 엄폐물(0.7m)은 눈높이(1.5m)보다 낮아 못 가리고, 전신 엄폐물(2.2m)만 가린다.
        // 1교시의 "허리=쏘며 숨기 / 전신=완전 은신" 구분이 바로 이 판정에서 실현된다
        Vector3 eye = transform.position + Vector3.up * eyeHeight;
        Vector3 targetEye = target.position + Vector3.up * eyeHeight;

        if (Physics.Raycast(eye, (targetEye - eye).normalized, distance, obstacleMask))
        {
            return false;
        }

        return true;
    }

    /// <summary>순찰 — 49차시 패턴 그대로, 속도만 profile.moveSpeed.</summary>
    private void Patrol()
    {
        if (patrolPoints == null || patrolPoints.Length == 0)
        {
            navAgent.isStopped = true;
            return;
        }

        if (waitTimer > 0f)
        {
            waitTimer -= Time.deltaTime;

            if (waitTimer <= 0f)
            {
                patrolIndex = (patrolIndex + 1) % patrolPoints.Length;
            }

            return;
        }

        navAgent.isStopped = false;
        navAgent.speed = profile.moveSpeed; // 평상시 속도 — 추적 속도와 분리된 성격 항목
        navAgent.SetDestination(patrolPoints[patrolIndex].position);

        // pathPending 가드 (49차시 진단 문제의 그 가드)
        if (!navAgent.pathPending && navAgent.remainingDistance <= navAgent.stoppingDistance + arrivalThreshold)
        {
            navAgent.isStopped = true;
            waitTimer = patrolWaitTime;
        }
    }

    private void LookAtTarget()
    {
        Vector3 direction = target.position - transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude < 0.01f)
        {
            return;
        }

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            Quaternion.LookRotation(direction),
            Time.deltaTime * 8f
        );
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }


    public void SetPatrolPoints(params Transform[] points)
    {
        for (int i = 0; i < patrolPoints.Length; i++) 
        {
            patrolPoints[i] = points[i];
        }
        
    }

    // ==================== Gizmo (5교시 + 레벨 3 부채꼴) ====================
    // 숫자 20이 몇 걸음인지는 그려봐야 안다 — 수치를 씬에 그려 설계·밸런싱을 눈으로 확인.
    // Selected 버전: 선택했을 때만, 에디터에서만. 빌드에는 아예 포함되지 않는 순수 개발용 코드.
    private void OnDrawGizmosSelected()
    {
        // Profile 미연결 상태에서 선택하면 NullReferenceException이 쏟아지므로 가드 (어제 Health 가드와 같은 정신)
        if (profile == null)
        {
            return;
        }

        // 감지 범위 (노랑) — 반지름의 출처가 하드코딩이 아니라 profile인 것이 오늘의 핵심.
        // Profile만 바꿔 끼우면 코드 수정 없이 원 크기가 즉시 바뀐다
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, profile.sightRange);

        // 공격 범위 (빨강) — Gizmos.color는 "현재 펜 색" 같은 정적 상태값이라 매번 바꿔줘야 한다
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, profile.attackRange);

        // [레벨 3] 시야 부채꼴 — 49차시 Gizmo 코드 그대로 재사용 (재사용은 실력)
        Vector3 left = Quaternion.Euler(0f, -profile.fieldOfView * 0.5f, 0f) * transform.forward;
        Vector3 right = Quaternion.Euler(0f, profile.fieldOfView * 0.5f, 0f) * transform.forward;

        Gizmos.color = Color.cyan;
        Gizmos.DrawRay(transform.position, left * profile.sightRange);
        Gizmos.DrawRay(transform.position, right * profile.sightRange);
    }


}
