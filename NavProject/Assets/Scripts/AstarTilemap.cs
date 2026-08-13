//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.Tilemaps;

//public class AstarTilemap : MonoBehaviour
//{
//    [Header("맵 크기")]
//    [SerializeField] private int width;
//    [SerializeField] private int height;

//    [Header("타일맵")]
//    [SerializeField] private Tilemap tilemap;
//    [SerializeField] private TileBase floorTile;
//    [SerializeField] private TileBase wallTile;
//    [SerializeField] private TileBase startTile;
//    [SerializeField] private TileBase goalTile;
//    [SerializeField] private TileBase pathTile;
//    [SerializeField] private TileBase findTile;

//    [Header("타일맵 설치 딜레이")]
//    [SerializeField] private float processDelay;
//    [SerializeField] private float pathFindingDelay;

//    private Vector3Int startPos;
//    private Vector3Int goalPos;
//    private bool hasStart;
//    private bool hasGoal;
//    private bool[,] obstacleMap; // A* 알고리즘에 사용할 장애물(맵) 정보.
//    private List<Vector3Int> wallPositions = new List<Vector3Int>(); // usingMap을 만들기 위한 장애물 정보를 담은 리스트.


//    void Start()
//    {
//        TilemapInit(); 
//    }

//    /// <summary>
//    /// // 타일맵을 지정한 크기로 만들고, 초기화 작업을 진행하는 메서드.
//    /// </summary>
//    private void TilemapInit()
//    {
//        tilemap.ClearAllTiles(); // 타일맵 전체 삭제

//        wallPositions.Clear(); // 장애물의 위치를 담은 리스트도 초기화.

//        // 바닥 타일 깔기
//        for (int x = 0; x < width; x++)
//        {
//            for (int y = 0; y < height; y++)
//            {
//                Vector3Int pos = new Vector3Int(x, y, 0);
//                tilemap.SetTile(pos, floorTile); // 위치와 배치할 타일을 받아 타일을 설치하는 메서드.
//            }
//        }

//        // 경계 타일은 벽으로 처리
//        for (int x = 0; x < width; x++)
//        {
//            SetWallTile(new Vector3Int(x, 0, 0));
//            SetWallTile(new Vector3Int(x, height - 1, 0));
//        }

//        for (int y = 1; y < height - 1; y++)
//        {
//            SetWallTile(new Vector3Int(0, y, 0));
//            SetWallTile(new Vector3Int(width - 1, y, 0));
//        }

//        Camera.main.transform.position = new Vector3(width / 2f, height / 2f, -10f); // 카메라는 타일맵의 중심으로
//        Camera.main.orthographicSize = (width + height) / 4; // 타일맵의 전체가 보이도록

//        // 시작점, 도착점 초기화
//        startPos = Vector3Int.zero;
//        goalPos = Vector3Int.zero;

//        // 시작점, 도착점 선택 여부 초기화
//        hasStart = false;
//        hasGoal = false;
//    }   

//    private void Update()
//    {
//        if (Input.GetMouseButtonDown(0)) 
//        {
//            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition); // 마우스 클릭으로 해당 지점의 포지션 가져옴.
//            Vector3Int clickedTile = tilemap.WorldToCell(mouseWorldPos); // 마우스 클릭으로 가져온 위치를 해당 타일의 위치로 변환하여 가져옴.

//            if (!hasStart) // 시작점 선택
//            {
//                startPos = clickedTile; 
//                tilemap.SetTile(startPos, startTile);
//                hasStart = true;
//            }
//            else if (!hasGoal && clickedTile != startPos) // 도착점 선택, 시작점과 같을 수 없음.
//            {
//                goalPos = clickedTile;
//                tilemap.SetTile(goalPos, goalTile);
//                hasGoal = true;
//            }
//            else if (clickedTile != startPos && clickedTile != goalPos) // 이후 장애물 선택, 시작점, 도착점과 같을 수 없음.
//            {
//                SetWallTile(clickedTile);
//            }
//        }

//        if (Input.GetKeyDown(KeyCode.Space) && hasStart && hasGoal) // 시작점, 도착점이 지정되고, Space 입력 받아 알고리즘 시작.
//        {
//            StartCoroutine(DrawPath());
//        }

//        if(Input.GetKeyDown(KeyCode.R)) // R 키 입력으로 타일맵 세팅 초기화.
//        {
//            TilemapInit(); 
//        }
//    }

//    /// <summary>
//    /// 장애물의 위치를 지정하고, 장애물의 위치를 저장하는 메서드.
//    /// </summary>
//    /// <param name="position">장애물을 설치할 위치</param>
//    private void SetWallTile(Vector3Int position) 
//    {
//        tilemap.SetTile(position, wallTile);
//        wallPositions.Add(position); // 장애물의 위치를 저장.
//    }

//    /// <summary>
//    /// A*알고리즘의 진행과정을 타일맵으로 보여주기 위한 코루틴.
//    /// </summary>
//    /// <returns></returns>
//    private IEnumerator DrawPath()
//    {
//        obstacleMap = AstarAlgorithm.ConvertToMap(wallPositions, width, height); // 장애물의 위치를 담은 리스트를 이용하여 A* 알고리즘을 적용할 수 있도록 맵의 형태로 만들어줌.

//        List<Vector2Int> path = AstarAlgorithm.FindPath(obstacleMap, (Vector2Int)startPos, (Vector2Int)goalPos); // A* 알고리즘을 통한 경로 탐색.
//        List<Vector2Int> pathlist = AstarAlgorithm.ShowFindProcess(obstacleMap, (Vector2Int)startPos, (Vector2Int)goalPos); // A* 알고리즘을 통해 탐색했던 모든 노드들을 저장한 리스트. 시각적으로 보여주기 위한 기능. 

//        foreach (Vector2Int pos in pathlist) // 최적의 경로가 아닌, 탐색했던 모든 노드들을 보여줌.
//        {
//            Vector3Int tilePos = new Vector3Int(pos.x, pos.y, 0);

//            yield return new WaitForSeconds(processDelay); // 점진적으로 진행하는 걸 보여주기 위한 딜레이

//            if (tilePos != startPos && tilePos != goalPos)
//            {
//                tilemap.SetTile(tilePos, findTile);
//            }
//        }

//        foreach (Vector2Int pos in path) // 탐색을 완료하고 나온 최적의 결과를 보여줌.
//        {
//            Vector3Int tilePos = new Vector3Int(pos.x, pos.y, 0);

//            yield return new WaitForSeconds(pathFindingDelay); // 점진적으로 진행하는 걸 보여주기 위한 딜레이

//            if (tilePos != startPos && tilePos != goalPos)
//            {
//                tilemap.SetTile(tilePos, pathTile);
//            }
//        }
//    }
//}
