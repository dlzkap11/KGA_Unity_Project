using UnityEngine;

/// <summary>
/// [51차시 5교시 — 요구사항 변경 "Health 0줄"] 피격 사운드 수신기.
///
/// "맞을 때 소리도 나면 좋겠어요"라는 새 요구를 구현한 결과:
///   새로 만든 곳 1(이 파일), Health 고친 곳 0줄.  ← 구조의 성적표
///
/// 구조가 HitFlash와 완전히 같다(동형의 수신기) — 다른 건 반응 내용(색 대신 소리)뿐.
/// 이렇게 "두 번째 수신기"는 5분 만에 끝난다는 것이 좋은 구조의 증거다 (실습 레벨 2의 메시지).
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class HitSound : MonoBehaviour
{
    [Header("사운드 설정")]
    [SerializeField] private AudioClip hitClip; // 피격음 클립 (패키지의 Sounds/Hit.wav)

    private Health health;
    private AudioSource audioSource;

    private void Awake()
    {
        health = GetComponent<Health>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        health.OnDamaged += HandleDamaged;
    }

    private void OnDisable()
    {
        health.OnDamaged -= HandleDamaged; // += / -= 는 반드시 짝
    }

    private void HandleDamaged(int current, int max)
    {
        // PlayOneShot: 재생 중인 소리를 끊지 않고 "겹쳐서" 재생 —
        // 여러 발을 빠르게 맞아도 소리가 잘리지 않는 이유
        if (hitClip != null)
        {
            audioSource.PlayOneShot(hitClip);
        }
    }
}
