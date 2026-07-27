using UnityEngine;

public class RepositionMap : MonoBehaviour
{
    [SerializeField] private float mapSize;
    [SerializeField] private Transform playerPos;

    private void Update()
    {
        MapReposition();
    }

    private void MapReposition()
    {
        // 맵의 현재위치와 플레이어 위치간의 차이를 계산.
        float diffX = playerPos.position.x - transform.position.x;
        float diffY = playerPos.position.y - transform.position.y;

        // 플레이어가 맵 중심을 기준으로 어느쪽에 있는지를 판단
        Vector3 directFromMapCenter = playerPos.position - transform.position;

        // 이번 단위에 실제로 맵이 옮겨졌는지를 기록
        bool isRepositioned = false;

        // X축을 검사
        // 플레이어가 맵 중심을 기준으로 가로로 MapSize만큼 거리가 벌어졌는지 확인
        if (Mathf.Abs(diffX) > mapSize)
        {
            if (directFromMapCenter.x < 0)
            {
                transform.Translate(Vector3.left * mapSize * 2);
            }
            else if (directFromMapCenter.x > 0)
            {
                transform.Translate(Vector3.right * mapSize * 2);
            }

            isRepositioned = true;
        }

        // Y축을 검사
        // 플레이어가 맵 중심을 기준으로 세로로 MapSize만큼 거리가 벌어졌는지 확인
        if (Mathf.Abs(diffY) > mapSize)
        {
            if (directFromMapCenter.y < 0)
            {
                transform.Translate(Vector3.down * mapSize * 2);
            }
            else if (directFromMapCenter.y > 0)
            {
                transform.Translate(Vector3.up * mapSize * 2);
            }

            isRepositioned = true;
        }
    }
}