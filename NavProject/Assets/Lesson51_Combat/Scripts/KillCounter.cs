using UnityEngine;

/// <summary>
/// [51차시 실습 레벨 2] 처치 수 카운터 — 역시 "Health 0줄" 수신기.
///
/// 구조는 HitFlash·HitSound와 완전히 같고, 새로운 지점은 "여러 Enemy를 구독"하는 것뿐:
///   Health[]를 받아 foreach로 += 를 걸고, 해제도 foreach로 -= — 짝 맞추기는 배열에서도 그대로.
///
/// ★ 2교시 isDead 가드가 여기서 회수된다: 그 가드가 없었다면 죽은 적을 계속 쏠 때마다
///   OnDied가 반복 발행되어 한 명 죽였는데 카운트가 5, 10씩 올라갔을 것이다.
///
/// (52차시 FPS의 "남은 적 카운트 → 게임 클리어 조건"으로 진화할 부품)
/// </summary>
public class KillCounter : MonoBehaviour
{
    [Header("감시 대상")]
    [SerializeField] private Health[] enemyHealths; // 씬의 Enemy Health들을 드래그

    private int killCount;

    private void OnEnable()
    {
        // 한 명 구독하는 코드를 foreach로 감싼 것뿐
        foreach (Health enemyHealth in enemyHealths)
        {
            if (enemyHealth != null)
            {
                enemyHealth.OnDied += HandleEnemyDied;
            }
        }
    }

    private void OnDisable()
    {
        // 해제도 똑같이 foreach로 — 짝은 배열에서도 짝
        foreach (Health enemyHealth in enemyHealths)
        {
            if (enemyHealth != null)
            {
                enemyHealth.OnDied -= HandleEnemyDied;
            }
        }
    }

    private void HandleEnemyDied()
    {
        killCount++;
        Debug.Log($"[KillCounter] 처치 수: {killCount}");
    }

    // UI 시스템 없이 화면에 간단히 표시 (정식 UI는 52차시 FPS에서 — Slider·TextMeshPro)
    private void OnGUI()
    {
        GUI.Label(new Rect(12f, 12f, 300f, 30f), $"처치 수: {killCount}");
    }
}
