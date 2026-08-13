using System.Collections.Generic;
using UnityEngine;

public class Node
{
    // 해당 칸의 좌표
    public Vector2Int Postion;

    // 지나갈 수 있는지 여부(벽 체크)
    public bool Walkable;

    // G = 시작점에서 이 노드까지 실제로 든 비용
    public int G;

    // H = 이 노드에서 목적지까지 예상거리
    public int H;

    // F = G + H
    public int F => G + H;

    // 부모 노드 : 어느 칸에서 왔는지 - 경로 추적용
    public Node Parent;


    public Node(Vector2Int pos, bool walkable)
    {
        Postion = pos;
        Walkable = walkable;
    }

}


public class Astar
{
    // 직선 이동 값
    private const int STRAIGHT_COST = 10;
    // 대각선 이동 값
    private const int DIAGONAL_COST = 14;

    private static readonly Vector2Int[] Directions =
    {
        new Vector2Int(0, 1), // 상
        new Vector2Int(0, -1), // 하
        new Vector2Int(-1, 0), // 좌
        new Vector2Int(1, 0), // 우
        new Vector2Int(-1, 1), // 좌상
        new Vector2Int(1, 1), // 우상
        new Vector2Int(-1, -1), // 좌하
        new Vector2Int(1, -1), // 우하
    };

    private readonly bool[,] map;
    private readonly int width;
    private readonly int height;

    public Astar(bool[,] map)
    {
        this.map = map;
        width = map.GetLength(0);
        height = map.GetLength(1);
    }

    // ** 경로를 탐색하는 함수 **
    public List<Vector2Int> FindPath(Vector2Int start, Vector2Int goal)
    {
        // 앞으로 탐색할 후보들
        List<Node> openList = new List<Node>();

        // 이미 확정되어서 다시 안봐도 되는 노드들
        List<Vector2Int> closedList = new List<Vector2Int>();

        // 같은 좌표의 Node를 매번 만들지 않도록 좌표를 통해 노드를 찾을 수 있도록 해준다.
        Dictionary<Vector2Int, Node> nodes = new Dictionary<Vector2Int, Node>();

        // 시작 노드의 G = 0 H = 휴리스틱 계산
        Node startNode = new Node(start, true)
        {
            G = 0,
            H = GetDistance(start, goal)
        };

        // 시작 노드를 openList에 추가
        openList.Add(startNode);
        nodes[start] = startNode;

        //      [반복] openList가 비어있지 않은 동안
        while(openList.Count > 0)
        {
            //① openList에서 F가 가장 낮은 노드를 고른다
            //     (F가 같으면 H가 낮은 쪽)
            Node current = openList[0];

            // F와 H를 비교해서 가장 낮은 것을 찾아준다.
            for(int i = 1; i < openList.Count; i++)
            {
                bool isBetterF = openList[i].F < current.F;
                bool isBetterH = openList[i].F == current.F && openList[i].H < current.H;

                if(isBetterF || isBetterH)
                {
                    current = openList[i];
                }
            }

            //② 그 노드를 openList에서 빼고 closedList에 넣는다  
            openList.Remove(current);
            closedList.Add(current.Postion);

            //③ 그 노드가 목적지면 → 경로를 역추적하고 종료
            if(current.Postion == goal)
            {
                return RetracePath(startNode, current);
            }
            //④ 그 노드의 이웃 칸들을 하나씩 확인한다
            foreach(Vector2Int dir in Directions)
            {
                Vector2Int neighborPos = current.Postion + dir;
                // 격자 밖이거나
                if (!IsInsiderGrid(neighborPos))
                    continue;
                // 벽이면 건너뛴다
                if (!map[neighborPos.x, neighborPos.y])
                    continue;
                // 이미 closedList에 있으면 건너뛴다
                if (closedList.Contains(neighborPos))
                    continue;

                // 대각선이면 인접한 수직, 수평이 모두 열려있는지 확인
                if (IsDiagonal(dir) && CanMoveDiagonally(current.Postion, dir))
                    continue;

                // 새 G = 현재 노드의 G +이동 비용(대각선 14 / 직선 10)
                int moveCost = IsDiagonal(dir) ? DIAGONAL_COST : STRAIGHT_COST;
                int newG = current.G + moveCost;

                // 현재 위치에 만들어진 노드가 존재하지 않는 경우
                if(!nodes.TryGetValue(neighborPos, out Node neighberNode))
                {
                    // 새로 노드를 만들어 준다.
                    neighberNode = new Node(neighborPos, true);
                    nodes[neighborPos] = neighberNode;
                }

                bool isOpenListExist = openList.Contains(neighberNode);

                // 새 G가 기존에 기록된 G보다 작고, openList에 없다면
                if(newG < neighberNode.G || !isOpenListExist)
                {
                    // G, H를 갱신하고  
                    neighberNode.G = newG;
                    neighberNode.H = GetDistance(neighborPos, goal);
                    
                    // 부모(어디서 왔는지)를 현재 노드로 기록하고  
                    neighberNode.Parent = current;

                    // openList에 없으면 추가한다
                    if(!openList.Contains(neighberNode))
                        openList.Add(neighberNode);

                }
            }
        }
        // openList가 비었는데 목적지에 도달하지 못했다면 -> 경로 없음
        return null;
    }

    // 경로 역추적 함수
    public List<Vector2Int> RetracePath(Node startNode, Node endNode)
    {
        // 경로 저장 리스트
        List<Vector2Int> path = new List<Vector2Int>();

        // 도착지에 도착했을 때부터 거슬러올라가므로, endNode부터 계산
        Node current = endNode;
        
        // 시작 노드에 도달할 때까지 부모를 따라 거슬러 올라감
        while(current != startNode)
        {
            path.Add(current.Postion);
            current = current.Parent;
        }
        // 시작 노드 포함
        path.Add(startNode.Postion);

        // 도착지점부터 역으로 추가되었기 때문에 역정렬을 해줘야한다.
        path.Reverse();

        return path;
    }

    // 휴리스틱 계산 함수
    // H = 장애물은 무시하고 계산
    private int GetDistance(Vector2Int from, Vector2Int to)
    {

        // X와의 거리와 Y와의 거리를 위치간의 차로 만들어주고, 절대값으로 값만 남겨둘 수 있도록 한다.
        int distX = Mathf.Abs(from.x - to.x);
        int distY = Mathf.Abs(from.y - to.y);

        // X와 Y간에 거리차의 크기에 따라서 대각선이 X의 값을 줄여줄지, Y값을 줄여줄지에 따라 달라짐
        if(distX > distY)
        {
            return DIAGONAL_COST * distY + STRAIGHT_COST * (distX - distY);
        }

        return DIAGONAL_COST * distX + STRAIGHT_COST * (distY - distX);

    }

    // 대각선 방향인지
    private bool IsDiagonal(Vector2Int dir)
    {
        return dir.x != 0 && dir.y != 0;
    }

    // 대각선 이동이 가능한지
    private bool CanMoveDiagonally(Vector2Int from, Vector2Int dir)
    {
        Vector2Int horizontal = from + new Vector2Int(dir.x, 0);
        Vector2Int vertical = from + new Vector2Int(0, dir.y);

        if(!IsInsiderGrid(horizontal) || !map[horizontal.x, horizontal.y])
            return false;

        if (!IsInsiderGrid(vertical) || !map[vertical.x, vertical.y])
            return false;

        return true;
    }

    // 내가 참조하는 맵 안인지
    private bool IsInsiderGrid(Vector2Int pos)
    {
        return pos.x >= 0 && pos.y >= 0 && pos.x < width && pos.y < height;
    }

}
