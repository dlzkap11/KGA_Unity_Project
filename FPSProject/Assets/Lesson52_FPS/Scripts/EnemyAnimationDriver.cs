using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// [52차시 5교시 — 배우에게 몸 입히기] Animator 연결 부품.
///
/// 애니메이션은 장식이 아니라 "적이 뭘 하는지 플레이어에게 알려주는 UI"다.
/// 이 부품이 하는 일 세 가지:
///   ① 매 프레임 이동 속도를 Animator의 Speed 파라미터로 (Idle ↔ Run 전이의 재료)
///   ② Health의 "맞았어" 방송 구독 → Hit 트리거   ┐
///   ③ Health의 "죽었어" 방송 구독 → Die 트리거   ┘ 51차시 수신기 패턴 그대로!
///
/// ★ "Health 0줄"의 세 번째 증명: 애니메이션을 연결하면서 Health는 한 글자도 안 고쳤다.
///   (51차시 사운드 0줄 → 52차시 탱커형 0곳 → 52차시 애니메이션 0줄)
///
/// ⚠ 함정 셋 (부록의 함정 5종 중):
///   - Animator 파라미터 이름 오타 → 에러 없이 모션만 안 나옴 (50차시 SetVariableValue 오타와 같은 세계)
///     → 그래서 이름을 const로 한 곳에 모아뒀다 (매직 스트링 방어)
///   - Apply Root Motion 켜짐 → NavMeshAgent와 "운전자가 둘"이 되어 미끄러짐 (48차시: 운전자는 하나)
///   - Die 상태에 Exit Time → 죽었다 되살아남 (51차시: 죽음은 상태이지 사건이 아니다)
///
/// ※ Health는 51차시 패키지의 부품 — Lesson51 임포트 필요.
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAnimationDriver : MonoBehaviour
{
    // Animator 파라미터 이름 — 문자열 연결은 오타를 안 잡아주므로 상수로 모아둔다
    private const string ParamSpeed = "Speed";
    private const string ParamHit = "Hit";
    private const string ParamDie = "Die";

    [Header("연결 대상")]
    [SerializeField] private Animator animator; // 모델(자식)의 Animator. 비워두면 자식에서 자동 검색, 그래도 없으면 조용히 대기

    private NavMeshAgent agent;
    private Health health; // 51차시 부품 — 방송 구독용 (없으면 Speed만 갱신)

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        health = GetComponent<Health>();

        // 모델을 자식으로 넣는 구성(피벗 보정 — 5교시 FAQ)을 기본으로 가정
        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }
    }

    private void OnEnable()
    {
        // 51차시 수신기 패턴 그대로 — 구독은 OnEnable에서
        if (health != null)
        {
            health.OnDamaged += HandleDamaged;
            health.OnDied += HandleDied;
        }
    }

    private void OnDisable()
    {
        // += / -= 는 반드시 짝
        if (health != null)
        {
            health.OnDamaged -= HandleDamaged;
            health.OnDied -= HandleDied;
        }
    }

    private void Update()
    {
        // Animator가 아직 없으면(캡슐 상태) 조용히 대기 — 모델을 넣는 순간부터 작동
        if (animator == null)
        {
            return;
        }

        // 이동 속도 → Speed 파라미터 (Idle↔Run 전이 조건 0.1과 비교됨)
        animator.SetFloat(ParamSpeed, agent.velocity.magnitude);
    }

    private void HandleDamaged(int current, int max)
    {
        if (animator != null) // Unity 오브젝트는 ?.가 아니라 if로! (51차시의 그 함정)
        {
            animator.SetTrigger(ParamHit);
        }
    }

    private void HandleDied()
    {
        if (animator != null)
        {
            animator.SetTrigger(ParamDie);
        }
    }
}
