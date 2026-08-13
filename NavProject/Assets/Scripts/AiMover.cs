using UnityEngine;
using UnityEngine.AI;

public class AiMover : MonoBehaviour
{
    private NavMeshAgent m_Agent;
    [SerializeField] Camera m_Camera;
    [SerializeField] LayerMask m_LayerMask;
    [SerializeField] Transform pos;

    [SerializeField] LayerMask groundLayer2;
    private int groundLayer;
    private int obstacleLayer;

    void Awake()
    {
        m_Agent = GetComponent<NavMeshAgent>();
        if (m_Camera == null)
            m_Camera = Camera.main;
        groundLayer = LayerMask.NameToLayer("Ground");
        obstacleLayer = LayerMask.NameToLayer("Obstacle");
    }

    private void Update()
    {
        //마우스버튼
        if (!Input.GetMouseButtonDown(0))
            return;

        Ray ray = m_Camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        bool raycastHit = Physics.Raycast(ray, out hit, 100.0f, m_LayerMask);
        Debug.DrawRay(m_Camera.transform.position, ray.direction * 100.0f, Color.red, 1.0f);
        

        int hitLayer = hit.collider.gameObject.layer;

        Debug.Log($"hitlayer : {hitLayer}, groundlayer : {(int)groundLayer2}, {1 << hitLayer}");
        if (((1 << hitLayer) & groundLayer2) != 0)
        {
            m_Agent.SetDestination(hit.point);
        }
        else
        {
            Debug.Log("장애물!");
        }
        /*
        if (hitLayer == groundLayer)
        {
            m_Agent.SetDestination(hit.point);
        }
        else if (hitLayer == obstacleLayer)
        {
            Debug.Log("장애물!");
        }
        */



    }
}
