using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class WatchTower : MonoBehaviour
{
    /*
    *  감시타워
    * 타워 오브젝트 하나와 주변에 굴러다니는ㄴ 큐브 여러개
    * 타워를 선택하면 씬뷰에서 범위가 원으로 표시
    * 일정한 주기마다 범위 안에 어떤 오브젝트가 들어와있는지 확인
    * 범위안 오브젝트는 색이 바뀌고 나가면 원래색
    * 범위 안에 몇개가 있는지 화면에 표시
    * 타워를 클릭하면 그 즉시 다시 확인
    */
    public const float RANGE = 5.0f;
    public Color color;
    public GameObject go;
    public Collider[] colliders;

    private float cycle = 0.0f;
    [SerializeField]
    private HashSet<Collider> previousColliders = new HashSet<Collider>();
    private Dictionary<string, Color> colors = new Dictionary<string, Color>();

    public Transform centerPoint;
    public float radius = 5f;
    public LayerMask targetLayer;


    void Update()
    {

        OnWatchOut();
        cycle += Time.deltaTime;
        if (cycle > 5.0f)
        {
            cycle = 0.0f;
            return;
        }

        /*
        colliders = Physics.OverlapSphere(transform.position, RANGE);
        foreach (Collider collider in colliders)
        {
            color = collider.GetComponent<Renderer>().material.color;
            collider.GetComponent<Renderer>().material.color = Color.darkRed;
        }
        */
        // 1. 구체 범위 내의 콜라이더 탐색 (OverlapSphere)

    }

    void OnExitOverlap(Collider exitedCollider)
    {

        exitedCollider.GetComponent<Renderer>().material.color = colors[exitedCollider.name];
        colors.Remove(exitedCollider.name);
        Debug.Log(exitedCollider.name + "가 OverlapSphere 영역 밖으로 나갔습니다.");
    }


    /*
    private void OnCollisionEnter(UnityEngine.Collision collision)
    {
        color = collision.gameObject.GetComponent<Renderer>().material.color;
        collision.gameObject.GetComponent<Renderer>().material.color = Color.darkRed;
        Debug.Log($"감지{collision.gameObject.name}");
    }

    private void OnCollisionExit(UnityEngine.Collision collision)
    {
        collision.gameObject.GetComponent<Renderer>().material.color = color;

        Debug.Log("나감");
    }
    */

    void OnWatchOut()
    {
        colliders = Physics.OverlapSphere(centerPoint.position, radius, targetLayer);
        foreach (Collider collider in colliders)
        {
            if (collider.GetComponent<Renderer>().material.color == Color.darkRed)
                continue;
            
            color = collider.GetComponent<Renderer>().material.color;
            if(!colors.ContainsKey(collider.name))
                colors.Add(collider.name, color);
            collider.GetComponent<Renderer>().material.color = Color.darkRed;
        }

        HashSet<Collider> currentColliders = new HashSet<Collider>(colliders);

        foreach (Collider col in previousColliders)
        {
            if (!colliders.Contains(col))
            {
                OnExitOverlap(col); // 밖으로 나갔을 때 실행할 함수
            }
        }


        previousColliders = currentColliders;
    }

    private void OnMouseDown()
    {
        OnWatchOut();
        Debug.Log("감지감지");
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, RANGE);
        //Gizmos.DrawSphere(transform.position, 3.0f);

    }
    private void OnGUI()
    {
        GUI.Label(new Rect(10, -5, 150, 50), $"{colliders.Length} 감지!");
    }
}
