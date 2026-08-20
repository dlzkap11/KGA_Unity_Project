using Unity.AI.Navigation;
using UnityEngine;

/// <summary>
/// [보조 스크립트] 패키지 임포트 직후 NavMesh를 아직 굽지 않은 상태에서도
/// 데모 씬이 바로 동작하도록 Play 시작 시 NavMeshSurface를 런타임 베이크한다.
/// 에디터에서 Bake 버튼으로 미리 구워두면 아무 일도 하지 않는다 (파란 영역 관찰은 에디터 Bake로).
/// ※ 49~51차시 패키지의 베이커와 같은 동작 — 클래스 이름 충돌 방지를 위해 별도 이름.
/// </summary>
[DefaultExecutionOrder(-100)]
[RequireComponent(typeof(NavMeshSurface))]
public class AutoNavMeshBaker : MonoBehaviour
{
    private void Awake()
    {
        NavMeshSurface surface = GetComponent<NavMeshSurface>();

        if (surface.navMeshData == null)
        {
            surface.BuildNavMesh();
        }
    }
}
