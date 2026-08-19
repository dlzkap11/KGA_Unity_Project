using System.Collections;
using UnityEngine;

/// <summary>
/// [51차시 실습 레벨 3] 맞은 자리에 데미지 숫자 띄우기 — 코루틴 + 이벤트 구독, 두 장치의 결합.
///
/// ⭐ 계약 확장의 딜레마 (이 레벨의 진짜 과제):
///   "맞은 정확한 위치"를 알려면 OnDamaged 이벤트에 위치를 실어야 하고, 그러면 Health를 고쳐야 한다.
///   → 이 구현은 "머리 위 고정 위치"로 타협해서 Health 0줄을 유지했다.
///   데미지량도 이벤트에 없지만, (직전 체력 − 현재 체력) 차분으로 계산해 역시 0줄을 지켰다.
///   계약 확장이 필요할 만큼 정밀한 연출이 필요해지면 그때 Health를 고치는 비용을 낸다 — 트레이드오프.
/// </summary>
public class DamagePopup : MonoBehaviour
{
    [Header("연출 설정")]
    [SerializeField] private float popupLifetime = 0.7f;  // 숫자가 떠 있는 시간
    [SerializeField] private float riseSpeed = 1.5f;      // 떠오르는 속도
    [SerializeField] private Color textColor = Color.yellow;

    private Health health;
    private int lastHealth = -1; // 데미지량 계산용 직전 체력 (-1 = 아직 한 번도 안 맞음)

    private void Awake()
    {
        health = GetComponent<Health>();
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
        // 타협 ②: 이벤트에 데미지량이 없으므로 차분으로 계산 (첫 피격이면 최대체력 기준)
        int previous = lastHealth < 0 ? max : lastHealth;
        int damageAmount = previous - current;
        lastHealth = current;

        SpawnPopup(damageAmount);
    }

    private void SpawnPopup(int damageAmount)
    {
        // 타협 ①: "맞은 정확한 위치" 대신 머리 위 고정 위치 (Health 0줄 유지)
        GameObject popup = new GameObject("DamagePopup");
        popup.transform.position = transform.position + Vector3.up * 2f;

        // 카메라를 바라보게(빌보드) — 카메라 회전을 그대로 따라가면 항상 정면으로 읽힌다
        if (Camera.main != null)
        {
            popup.transform.rotation = Camera.main.transform.rotation;
        }

        // 레거시 TextMesh — UI 캔버스 없이 월드 공간에 글자를 띄우는 가장 간단한 방법
        TextMesh textMesh = popup.AddComponent<TextMesh>();
        textMesh.text = damageAmount.ToString();
        textMesh.color = textColor;
        textMesh.characterSize = 0.35f;
        textMesh.fontSize = 48;
        textMesh.anchor = TextAnchor.MiddleCenter;
        textMesh.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf"); // Unity 6 내장 폰트

        // 수명 종료 예약 — 이 컴포넌트(적)가 먼저 파괴돼도 팝업은 스스로 사라진다 (유령 방지 백업)
        Destroy(popup, popupLifetime);

        // 코루틴: 떠오르는 연출 (4교시 HitFeedback과 같은 장치 — 매 프레임 조금씩, 시간이 되면 끝)
        StartCoroutine(RisePopup(popup.transform));
    }

    private IEnumerator RisePopup(Transform popupTransform)
    {
        float elapsed = 0f;

        // popupTransform이 Destroy로 사라지면(null) 즉시 중단
        while (elapsed < popupLifetime && popupTransform != null)
        {
            popupTransform.position += Vector3.up * (riseSpeed * Time.deltaTime);
            elapsed += Time.deltaTime;
            yield return null; // "다음 프레임에 다시 오라"
        }
    }
}
