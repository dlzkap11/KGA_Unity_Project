using UnityEngine;
using System.Collections.Generic;

public class ChunkSpawner : MonoBehaviour
{
    // 플레이어의 위치
    [SerializeField] private Transform playerPos;
    // 생성할 맵 프리팹
    [SerializeField] private GameObject chunkPrefab;
    // 맵 프리팹의 사이즈
    [SerializeField] private float chunkSize = 5f;
    // 생성할 청크의 갯수
    [SerializeField] private int chunkCount;

    // 지금까지 생성한 청크를 저장하고, 어느위치에 저장되어있는지 확인할 수 있어야함.
    private Dictionary<Vector2Int, GameObject> activeChunk = new Dictionary<Vector2Int, GameObject>();

    private void Update()
    {
        // 플레이어의 위치를 기준으로 현재 위치를 정수로 만들어줌.
        Vector2Int playerChunkPos = new Vector2Int
            (
                Mathf.RoundToInt(playerPos.position.x / chunkSize),
                Mathf.RoundToInt(playerPos.position.y / chunkSize)
            );

        // 좌우 청크의 갯수만큼 확인
        for (int x = -chunkCount; x <= chunkCount; x++)
        {
            for (int y = -chunkCount; y <= chunkCount; y++)
            {

                Vector2Int coord = new Vector2Int(playerChunkPos.x + x, playerChunkPos.y + y);
                // 확인해서 없으면 
                if (!activeChunk.ContainsKey(coord))
                {
                    // 청크생성
                    SpawnChunk(coord);
                }
            }
        }

        RemoveFarChunk(playerChunkPos);
    }

    private void SpawnChunk(Vector2Int playerCoord)
    {
        // 생성할 위치를 월드포지션 Vector3 값으로 변환. 칸 좌표 * chunkSize로 해당 위치 좌표 확인
        Vector3 worldPos = new Vector3(playerCoord.x * chunkSize, playerCoord.y * chunkSize, 0);

        // 계산한 위치에 chunkPrefab 생성
        GameObject chunk = Instantiate(chunkPrefab, worldPos, Quaternion.identity);

        // 현재 생성된 위치에 저장
        activeChunk.Add(playerCoord, chunk);
    }

    private void RemoveFarChunk(Vector2Int chunkCoord)
    {
        // 현재 Update 에서 activeChunk를 순회중이므로 순회가 끝나고 삭제할 수 있도록 모아둠
        List<Vector2Int> removeList = new List<Vector2Int>();

        // 현재 존재하는 청크중에
        foreach (var chunk in activeChunk)
        {

            // 가장 멀리있는 것을 비교해서 찾음(좌,우,상,하 중요하지 않음)
            int distance = Mathf.Max
                (
                    Mathf.Abs(chunk.Key.x - chunkCoord.x),
                    Mathf.Abs(chunk.Key.y - chunkCoord.y)
                );

            // 그것보다 한칸더 멀리있는것을
            if (distance > chunkCount + 1)
            {
                // 삭제 리스트에 올림
                removeList.Add(chunk.Key);
            }
        }

        foreach (Vector2Int coord in removeList)
        {

            // 청크 삭제
            Destroy(activeChunk[coord]);
            // 딕셔너리에서도 삭제
            activeChunk.Remove(coord);
        }
    }


}