using UnityEngine;
using UnityEngine.Tilemaps;
public class TileMapContoroller : MonoBehaviour
{
    [SerializeField] private Tilemap tileMap;
    [SerializeField] private TileBase groundTile;


    void Start()
    {
        for (int i = 0; i < 100; i++)
        {
            Vector3Int cellPos = new Vector3Int(i, 0, 0);
            tileMap.SetTile(cellPos, groundTile);
        }
    }

    
    void Update()
    {
        
    }
}
