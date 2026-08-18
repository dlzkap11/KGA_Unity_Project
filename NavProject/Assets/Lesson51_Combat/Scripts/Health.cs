using System;
using UnityEngine;

/// <summary>
/// [51차시 1교시] 공통 규칙서 — Player와 Enemy 양쪽에 꽂히는 같은 부품 (값만 다르게: Player 100 / Enemy 30).
///
/// 계약 두 줄:
///   ① 맞으면 방송한다 → OnDamaged(현재체력, 최대체력)
///   ② 죽으면 방송한다 → OnDied()
///   누가 듣는지는 모른다. 색·소리·UI는 계약에 없다.
///
/// ★ 이 스크립트에 색·소리·UI 코드가 단 한 줄도 없다는 것이 존재 이유이자,
///   4교시 "Health 0글자 도전"과 5교시 "0줄 요구사항 변경"이 성립하는 전제 조건이다.
///   연출이 필요하면 이 방송을 구독하는 수신기(HitFlash·HitSound·KillCounter…)를 새로 만들면 된다.
/// </summary>
public class Health : MonoBehaviour, IDamageable
{
    [Header("체력 설정")]
    [SerializeField] private int maxHealth = 100;   // Player 100 / Enemy 30 — 같은 스크립트, 값만 다르게

    // ── 애니메이션 확장 자리 (지금은 캡슐이라 미사용 — 모델을 붙일 때 주석 해제) ──
    // [SerializeField] private Animator animator; // "Hit"/"Die" 트리거를 가진 Animator 연결
    //
    // ⚠ 주석 해제 시 주의: animator?.SetTrigger(...) 처럼 ?.를 쓰면 안 된다!
    //   Inspector에서 비워둔 Unity 오브젝트 필드는 진짜 null이 아니라 "가짜 null"이라서
    //   ?.가 그냥 통과 → UnassignedReferenceException이 TakeDamage 중간에서 터져
    //   방송·로그가 전부 멈춘다 (= 쏴도 아무 반응 없는 증상).
    //   Unity 오브젝트는 반드시  if (animator != null)  로 검사할 것 —
    //   Unity가 ==를 재정의해서 가짜 null도 걸러준다. (?.는 순수 C# 객체·이벤트에만)

    // "방송 채널" — event 키워드 덕분에 클래스 밖에서는 += / -= 구독만 허용된다
    // (통째로 갈아치우거나 남이 대신 Invoke하는 것은 컴파일 에러 — 방송 버튼은 Health만 누를 수 있다)
    public event Action<int, int> OnDamaged;   // (현재 체력, 최대 체력) — "나 맞았어" 방송
    public event Action OnDied;                // "나 죽었어" 방송

    private int currentHealth;
    private bool isDead; // 중복 사망 방지 가드 — "죽음은 사건이 아니라 상태다"

    /// <summary>밖에서는 읽기만 — 실습 레벨 1에서 적 두뇌 판정 맨 위에 쓰는 값.</summary>
    public bool IsDead => isDead;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount, GameObject attacker)
    {
        // 가드 절: 이미 죽었으면 그냥 종료.
        // 이 한 줄이 없으면 죽은 대상을 계속 때릴 때마다 Die()가 다시 불려 사망 방송이 중복된다
        // → 실습의 "킬 카운터가 5씩 올라가는" 버그의 씨앗 (2교시 시연 내용)
        if (isDead)
        {
            return;
        }

        // Mathf.Max: 둘 중 큰 값 — 빼기 결과가 음수여도 체력은 항상 0 이상
        currentHealth = Mathf.Max(0, currentHealth - amount);

        // ── 애니메이션 확장 자리: 모델·Animator를 붙이면 주석 해제 (?.가 아니라 if로!) ──
        // if (animator != null) { animator.SetTrigger("Hit"); }

        // 수업 확인용 로그 (Enemy 30 → 20 → 10 → 0 감소 확인)
        Debug.Log($"[Health:{name}] {currentHealth}/{maxHealth} (공격자: {attacker?.name})");

        // "나 맞았어" 방송 — ?.는 구독자가 0명이면 조용히 건너뛰라는 가드 (50차시 agent?.와 문법·이유 동일)
        OnDamaged?.Invoke(currentHealth, maxHealth);

        if (currentHealth == 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;

        // ── 애니메이션 확장 자리: 모델·Animator를 붙이면 주석 해제 (?.가 아니라 if로!) ──
        // if (animator != null) { animator.SetTrigger("Die"); }

        Debug.Log($"[Health:{name}] 사망");

        // "나 죽었어" 방송 — 이동·공격 부품들이 이 방송을 구독해 스스로 꺼진다 (5교시 뒷정리)
        OnDied?.Invoke();

        // 뒷정리 ①: 내 몸의 모든 Collider를 끈다.
        // "죽음은 로직의 끝이지, 물리의 끝이 아니다 — 몸은 직접 꺼야 한다."
        // 이 블록이 없으면 시체가 총알을 대신 맞는 "시체 방패" 버그 (5교시 시연 내용)
        foreach (Collider bodyCollider in GetComponentsInChildren<Collider>())
        {
            bodyCollider.enabled = false;
        }
    }
}
