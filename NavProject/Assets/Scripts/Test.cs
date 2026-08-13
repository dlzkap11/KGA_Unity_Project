using UnityEngine;
using UnityEngine.AI;

public class Test : MonoBehaviour
{
    // NavMesh
    [SerializeField] private Camera _camera;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private LayerMask layerMask;

    private LayerMask groundMask;
    void Start()
    {
        _camera = Camera.main;
        agent = GetComponent<NavMeshAgent>();
        groundMask = LayerMask.GetMask("Ground");
    }

    
    void Update()
    {
        //Ray
        //까메라에서 레이저쏘기

        if (!Input.GetMouseButtonDown(0))
            return;

        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        RaycastHit[] hits;
        hits = Physics.RaycastAll(ray, 100f, layerMask);
        Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red, 1f);
        foreach(RaycastHit h in hits)
        {
            Debug.Log("관통 레이!" + h.collider.gameObject.name);
        }
        /*
        if(Physics.Raycast(ray, out hit, 100f, layerMask))
        {
            int hitLayer = hit.collider.gameObject.layer;
            if (((1 << hitLayer) & groundMask) != 0)
                agent.SetDestination(hit.point);
            else
            {
                Debug.Log("바닥이 아님!");
            }
        }
        */
        


    }
}
