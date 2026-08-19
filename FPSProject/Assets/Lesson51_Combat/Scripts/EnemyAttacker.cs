using UnityEngine;

/// <summary>
/// [51차시 3교시] Enemy의 이빨 — 공격 범위(Trigger) 안에 Player가 머무는 동안 일정 간격으로 문다.
///
/// 대칭 구조:
///   Player: 입력(클릭) → Raycast  → IDamageable.TakeDamage
///   Enemy : 판단(AI)   → Trigger  → IDamageable.TakeDamage
///           ↑ 결정은 다르다        ↑ 규칙은 같다
///
/// 부착 위치: Enemy의 자식 오브젝트(AttackRange — Sphere Collider + Is Trigger, 49차시 재사용).
///
/// ⚠ OnTriggerStay는 "머무는 동안 매 프레임"(초당 수십 번) 호출된다.
///   쿨다운 가드 없이 짜면 Player 체력 100이 2초 안에 증발하는 난타 사고가 난다 (3교시 자연 발생 고장).
/// </summary>
public class EnemyAttacker : MonoBehaviour
{
    [Header("공격 설정")]
    [SerializeField] private int damage = 5;             // 한 번 물 때 피해량
    [SerializeField] private float attackInterval = 1f;  // 공격 사이 최소 간격(초) — 이 값이 난타를 막는다

    private float nextAttackTime; // "이 시각이 되기 전엔 또 때리지 마라"는 예약 시각
    private Health ownerHealth;   // 내 본체(부모)의 체력 — 죽으면 공격이 스스로 꺼지기 위한 참조

    private void Awake()
    {
        // 이 스크립트는 자식(AttackRange)에 붙으므로 본체의 Health는 부모에서 찾는다
        ownerHealth = GetComponentInParent<Health>();
    }

    private void OnEnable()
    {
        // 뒷정리 ③: 본체가 죽으면 공격도 스스로 꺼진다 — OnDied 구독 (5교시)
        if (ownerHealth != null)
        {
            ownerHealth.OnDied += HandleOwnerDied;
        }
    }

    private void OnDisable()
    {
        // += / -= 는 반드시 짝 (4교시 구독 해제 유령)
        if (ownerHealth != null)
        {
            ownerHealth.OnDied -= HandleOwnerDied;
        }
    }

    private void HandleOwnerDied()
    {
        enabled = false; // 죽은 적은 더 이상 물지 않는다
    }

    // Enter(들어온 순간 한 번)가 아니라 Stay — "머무는 동안 매 프레임" 호출
    private void OnTriggerStay(Collider other)
    {
        // 컴포넌트가 꺼져 있으면 물지 않는다 (OnTriggerStay는 enabled=false여도 호출될 수 있으므로 명시 가드)
        if (!enabled)
        {
            return;
        }

        // 태그 필터 가드 — 없으면 자기 자신·벽 등 아무것에나 반응한다
        if (!other.CompareTag("Player"))
        {
            return;
        }

        // 쿨다운 가드 — Time.time(게임 시작 후 흐른 초)이 예약 시각 전이면 그냥 종료.
        // 1교시 isDead 가드, 49차시 pathPending 가드와 같은 자리, 같은 정신
        if (Time.time < nextAttackTime)
        {
            return;
        }

        // TryGetComponent = GetComponent + null 체크 두 줄을 한 줄로 합친 문법
        if (other.TryGetComponent(out IDamageable damageable))
        {
            damageable.TakeDamage(damage, gameObject);

            // 공격이 실제로 나간 "이후"에만 예약 시각 갱신 — 가드에 걸린 프레임에는 갱신하지 않는 게 포인트
            nextAttackTime = Time.time + attackInterval;
        }
    }
}
