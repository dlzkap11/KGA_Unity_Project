using System.Collections.Generic;
using UnityEngine;

public class AstarAlgorithm
{
    public class Node
    {

    }

    // 이동 가능한 방향 (대각선 포함 => 유클리드 거리)
    private static readonly Vector2Int[] Directions = new Vector2Int[]
    {
        new Vector2Int(1, 0),   // 오른쪽
        new Vector2Int(-1, 0),  // 왼쪽
        new Vector2Int(0, 1),   // 위
        new Vector2Int(0, -1),  // 아래
        new Vector2Int(1, 1),   // 오른쪽 위
        new Vector2Int(-1, 1),  // 왼쪽 위
        new Vector2Int(1, -1),  // 오른쪽 아래
        new Vector2Int(-1, -1)  // 왼쪽 아래
    };

    /// <summary>
    /// A* 알고리즘을 통해 시작점부터 도착점까지 최적의 경로를 탐색하여 반환하는 메서드.
    /// </summary>
    /// <param name="map">장애물의 정보를 담은 맵</param>
    /// <param name="start">시작 지점</param>
    /// <param name="goal">목표 지점</param>
    /// <returns> 최단 경로 리스트 (Vector2Int)</returns>
    public static List<Vector2Int> FindPath(bool[,] map, Vector2Int start, Vector2Int goal)
    {
        Astar astar = new Astar(map);
        return astar.FindPath(start, goal);
    }

    /// <summary>
    /// 탐색 과정을 보여주기 위해 방문한 모든 노드를 저장하여 반환하는 메서드.
    /// </summary>
    /// <param name="map">장애물의 정보를 담은 맵</param>
    /// <param name="start">시작 지점</param>
    /// <param name="goal">목표 지점</param>
    /// <returns></returns>
    public static List<Vector2Int> ShowFindProcess(bool[,] map, Vector2Int start, Vector2Int goal)
    {


    }

    /// <summary>
    /// 경로 탐색이 끝난 후, 결과로 나온 경로를 역추적하여 반환하는 메서드.
    /// </summary>
    private static List<Vector2Int> BuildPath(Node endNode)
    {

    }

    /// <summary>
    /// 휴리스틱 함수 (유클리드 거리 사용)
    /// </summary>
    private static float Heuristic(Vector2Int a, Vector2Int b)
    {

    }

    /// <summary>
    /// 장애물 정보로 받은 맵을 이용하여 이동이 가능한지 확인하는 메서드.
    /// </summary>
    private static bool IsValid(bool[,] map, Vector2Int pos)
    {

    }

    /// <summary>
    /// 타일맵에서 장애물 정보를 받아 2차원 bool 배열로 변환하는 메서드.
    /// </summary>
    public static bool[,] ConvertToMap(List<Vector3Int> wallPositions, int width, int height)
    {

    }
}
