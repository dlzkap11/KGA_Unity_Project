using UnityEngine;

/// <summary>
/// [52차시 5교시] 미니맵 카메라 따라가기.
///
/// 구조: 미니맵 카메라(Orthographic) → RenderTexture → Canvas의 RawImage가 화면 한구석에 표시.
///
/// - x·z만 따라가고 y(height)는 고정 — height는 맵 전체가 아니라 주변이 적당히 담기는 값을 실험으로.
/// - 회전을 다루는 줄이 아예 없는 것 자체가 설계: transform.rotation을 한 번도 안 건드리므로
///   몸이 아무리 돌아도 미니맵의 "위쪽"은 항상 고정이다.
///   (레벨 3 "미니맵 회전"은 이 스크립트가 아니라 플레이어 아이콘 쪽을 돌린다 — 카메라는 고정, 아이콘만 회전)
/// </summary>
public class MinimapFollow : MonoBehaviour
{
    [Header("추적 설정")]
    [SerializeField] private Transform target;  // 따라갈 대상(Player)
    [SerializeField] private float height = 20f; // 내려다보는 높이 — 실험으로 찾는 값

    private void LateUpdate() // 플레이어 이동이 끝난 뒤에 따라가도록 LateUpdate
    {
        // 가드 절 — Profile 가드, Health 가드와 같은 정신
        if (target == null)
        {
            return;
        }

        transform.position = new Vector3(target.position.x, height, target.position.z);
        // 회전은 여기서 건드리지 않는다 — Inspector에서 정한 top-down 각도(X=90)를 그대로 유지
    }
}
