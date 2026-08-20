using UnityEngine;

/// <summary>
/// [52차시 4교시] 전투 구역 진입 Trigger — "들어가면 깨어나는" 장치.
///
/// 1교시 설계의 "시작은 안전"을 코드로 지키는 부품:
///   구역의 적들은 비활성 상태로 잠들어 있다가, Player가 이 Trigger에 들어오는 순간 깨어난다.
///
/// ⚠ 이 Trigger에는 "그 구역의 적만" 연결할 것 —
///   씬의 모든 적을 연결하면 구역 2에 들어갔는데 구역 1도 깨어나는 통합 충돌이 난다.
/// 3종 세트(49·51차시 그대로): Is Trigger 체크 / Player에 Rigidbody / 태그 필터.
/// </summary>
[RequireComponent(typeof(BoxCollider))]
public class CombatZoneTrigger : MonoBehaviour
{
    [Header("이 구역에서 깨어날 적들")]
    [SerializeField] private GameObject[] zoneEnemies; // 비활성 상태로 배치된 "이 구역의" 적들만 드래그

    private bool triggered; // 한 번 깨어난 구역은 다시 발동하지 않는다 (가드)

    private void OnTriggerEnter(Collider other)
    {
        // 이미 발동했으면 종료 (가드 절)
        if (triggered)
        {
            return;
        }

        // Player만 반응 (태그 필터)
        if (!other.CompareTag("Player"))
        {
            return;
        }

        triggered = true;

        foreach (GameObject enemy in zoneEnemies)
        {
            if (enemy != null)
            {
                enemy.SetActive(true); // 깨어나라!
            }
        }

        Debug.Log($"[CombatZone:{name}] 전투 구역 활성화 — 적 {zoneEnemies.Length}명 기상");
    }

    // 구역 범위를 씬 뷰에서 항상 보이게 (에디터 전용 — 빌드에는 포함 안 됨)
    private void OnDrawGizmos()
    {
        if (TryGetComponent(out BoxCollider box))
        {
            Gizmos.color = new Color(1f, 0.9f, 0.2f, 0.9f);
            Gizmos.matrix = transform.localToWorldMatrix; // 회전·스케일까지 반영해 그리기
            Gizmos.DrawWireCube(box.center, box.size);
        }
    }
}
