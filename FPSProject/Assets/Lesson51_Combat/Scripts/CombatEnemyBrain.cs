using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// [51차시] 전투 데모용 최소 적 두뇌 — 순찰 / 추적 / 공격 거리 정지.
///
/// ※ 오늘의 주제는 전투(몸과 규칙)이므로 두뇌는 최소화했다.
///   시야각·수색 등 정교한 판단은 49차시(EnemyChaser)·50차시(SimpleEnemyBrain/그래프) 참고.
///   50차시 씬에 이어서 쓸 때는 그쪽 두뇌의 판정 맨 위에 IsDead 가드만 추가하면 된다.
///
/// ★ 실습 레벨 1의 핵심 연결이 이 스크립트에 들어 있다:
///   ① 판정 사슬 맨 위 → if (health.IsDead) return;   ("죽음이 모든 판단을 이긴다" — 위에 쓴 if가 이긴다)
///   ② OnDied 구독으로 이동 정지                        (뒷정리 ② — 방송 구조 그대로)
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
public class CombatEnemyBrain : MonoBehaviour
{
    [Header("추적 대상")]
    [SerializeField] private Transform target; // 비워두면 Awake에서 Player 태그 자동 검색

    [Header("판정 거리")]
    [SerializeField] private float detectionRange;    // 이 거리 안이면 추적 (데모용 단순 거리 감지)
    [SerializeField] private float attackRange;      // 이 거리 안이면 멈춰서 노려봄 (실제 공격은 EnemyAttacker의 Trigger가)
    [SerializeField] private float arrivalThreshold = 0.5f; // 순찰 도착 판정 여유

    [Header("순찰 설정 (49차시 패턴 재사용)")]
    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private float patrolWaitTime = 2f;

    [SerializeField] private FPSEnemyData enemyData;
    private NavMeshAgent navAgent;
    private Health health;      // 판정 맨 위의 IsDead 가드용
    private int patrolIndex;
    private float waitTimer;

    private void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();
        health = GetComponent<Health>();

        detectionRange = enemyData.sightRange;
        attackRange = enemyData.attackRange;

        if (target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                target = player.transform;
            }
        }
    }

    private void OnEnable()
    {
        // 뒷정리 ②: 죽으면 이동이 스스로 멈춘다 — HitFlash와 같은 구독 패턴
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
        // isStopped: 경로·목적지는 유지한 채 "이동만" 멈춤 (되살릴 일이 없어도 enabled보다 안전)
        navAgent.isStopped = true;
    }

    private void Update()
    {
        // ★ 판정 사슬 맨 위: 죽음이 모든 판단을 이긴다 (50차시 설계표 "Die는 맨 위"가 여기서 회수됨).
        //   이 가드가 없으면 "죽은 Enemy가 계속 쫓아오는" 통합 충돌이 난다 (실습 레벨 1 함정 표)
        if (health != null && health.IsDead)
        {
            return;
        }

        float distance = target == null
            ? float.MaxValue
            : Vector3.Distance(transform.position, target.position);

        if (distance <= attackRange)
        {
            // [공격 자리] 멈춰서 노려본다 — 무는 것은 EnemyAttacker(Trigger)의 몫 (결정과 실행의 분리)
            navAgent.isStopped = true;
            LookAtTarget();
        }
        else if (distance <= detectionRange)
        {
            // [추적]
            navAgent.isStopped = false;
            navAgent.SetDestination(target.position);
        }
        else
        {
            // [순찰]
            Patrol();
        }
    }

    private void Patrol()
    {
        // 가드: 초소가 없으면 그냥 서 있는다
        if (patrolPoints == null || patrolPoints.Length == 0)
        {
            navAgent.isStopped = true;
            return;
        }

        // 초소 대기 중이면 타이머만 흘린다
        if (waitTimer > 0f)
        {
            waitTimer -= Time.deltaTime;

            if (waitTimer <= 0f)
            {
                patrolIndex = (patrolIndex + 1) % patrolPoints.Length; // % 순환
            }

            return;
        }

        navAgent.isStopped = false;
        navAgent.SetDestination(patrolPoints[patrolIndex].position);

        // 도착 판정 — 49차시의 pathPending 가드 그대로 (경로 계산이 끝나기 전의 remainingDistance는 믿지 않는다)
        if (!navAgent.pathPending && navAgent.remainingDistance <= navAgent.stoppingDistance + arrivalThreshold)
        {
            navAgent.isStopped = true;
            waitTimer = patrolWaitTime;
        }
    }

    private void LookAtTarget()
    {
        if (target == null)
        {
            return;
        }

        Vector3 direction = target.position - transform.position;
        direction.y = 0f; // 수평 회전만

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
}
