using System.Collections;
using UnityEngine;

/// <summary>
/// [51차시 4교시 — "Health 0글자" 도전] 피격 깜빡임 수신기.
///
/// Health의 "나 맞았어" 방송(OnDamaged)을 구독해서 0.1초 빨갛게 물들였다 되돌린다.
/// ★ 이 스크립트 전체에 Health를 직접 수정하는 줄이 단 하나도 없다 —
///   오늘 목표인 "Health 0글자"를 코드로 증명하는 예시.
///
/// 생명주기 관례 (28차시):
///   - "한 번만 하면 되는 일"(캐싱·초기값 저장) → Awake
///   - "켜질 때마다 다시 해야 하는 일"(이벤트 구독) → OnEnable  (Awake가 OnEnable보다 항상 먼저)
/// </summary>
public class HitFlash : MonoBehaviour
{
    [Header("연출 설정")]
    [SerializeField] private Renderer bodyRenderer;   // 색을 바꿀 대상
    [SerializeField] private float flashDuration = 0.1f;

    private Health health;             // Awake에서 캐싱, OnEnable의 구독에 사용
    private Coroutine feedbackRoutine; // 겹침 가드용 — 진행 중인 코루틴 참조
    private Color originalColor;

    private void Awake()
    {
        health = GetComponent<Health>();

        // .material 첫 접근 시 Unity가 원본을 복제해 "이 오브젝트 전용 인스턴스"를 만들어 준다
        // → 색을 바꿔도 다른 적의 색은 안 바뀐다. (sharedMaterial을 쓰면 씬의 전부가 같이 빨개짐 — 절대 금지)
        originalColor = bodyRenderer.material.color;
    }

    private void OnEnable()
    {
        health.OnDamaged += HandleDamaged; // 방송 구독 — 수신기를 켠다
    }

    private void OnDisable()
    {
        // 반드시 짝으로! 해제를 빼먹으면 이 오브젝트가 파괴돼도 Health의 구독자 명단에 이름이 남아
        // 다음 피격 때 MissingReferenceException — "방송국은 해지 신청 없인 명단에서 이름을 지우지 않는다"
        health.OnDamaged -= HandleDamaged;
    }

    private void HandleDamaged(int current, int max)
    {
        // 겹침 가드: 0.1초가 지나기 전에 또 맞으면, 먼저 시작된 코루틴이 "원래 색 복원"을
        // 나중에 실행해 빨간 채로 멈추는 버그가 난다 → 기존 것을 먼저 멈추고 새로 시작
        if (feedbackRoutine != null)
        {
            StopCoroutine(feedbackRoutine);
        }

        feedbackRoutine = StartCoroutine(HitFeedback());
    }

    // 코루틴: 일반 함수가 "전화 통화"(끊을 때까지 한 번에)라면, 코루틴은 "문자 대화" —
    // yield return에서 멈췄다가, 조건이 되면 "그 줄 다음"부터 이어서. (스레드가 아니다! 여전히 메인 스레드)
    private IEnumerator HitFeedback()
    {
        bodyRenderer.material.color = Color.red;
        yield return new WaitForSeconds(flashDuration); // 여기서 멈추고, 시간이 되면 다음 줄부터
        bodyRenderer.material.color = originalColor;
    }
    // ※ 호출은 반드시 StartCoroutine(HitFeedback()) — 그냥 HitFeedback()이라고 부르면
    //   yield를 무시하고 아무 일도 일어나지 않는다 (오늘 실수 표 단골손님)
}
