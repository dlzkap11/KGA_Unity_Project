using UnityEngine;
using System.Collections.Generic;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private GameObject pooledPrefab;
    [SerializeField] private int poolSize;


    //Queue, List 등 무엇을 쓰는지에 대한 이유는 설명할 수 있어야한다
    private Queue<GameObject> poolQueue = new Queue<GameObject>();

    private List<GameObject> poolList = new List<GameObject>();
    // Add, RemoveAt(0) 첫빠따로 하면 된다.

    void Start()
    {
        CreatePool(poolSize);
    }

    public void CreatePool(int size)
    {
        for (int i = 0; i < size; i++)
        {
            // 오브젝트 풀링할 프리펩을 풀 크기만큼 만들어 준다. -> 미리 생성
            GameObject go = Instantiate(pooledPrefab);
            go.SetActive(false);
            poolQueue.Enqueue(go);
        }
    }
    // 풀에서 가져다 쓰기
    public GameObject GetObj()
    {
        GameObject go;

        // 풀에 있는 오브젝트가 부족한 상황일 때 어떻게 할 것인가
        if (poolQueue.Count <= 0)
        {
            // return null; => 그냥 리턴

            // 풀사이즈에 1/2만큼 더 생성해두기
            CreatePool(poolSize / 2);
            
        }

        go = poolQueue.Dequeue();
        go.SetActive(true);

        return go;
    }
    
    //다시 풀로 반환
    public void Return(GameObject go)
    {
        go.SetActive(false);
        poolQueue.Enqueue(go);
    }
}
