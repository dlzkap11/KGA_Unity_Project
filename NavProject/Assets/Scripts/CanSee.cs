using System.Runtime.CompilerServices;
using Unity.Behavior;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CanSee : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private BehaviorGraphAgent bga;
    public bool isCanSee { get; private set; }
    [SerializeField] private float detectRange;

    [Header("감지")]
    [SerializeField] private float viewAngle;
    [SerializeField] private float eyeHeight;
    [SerializeField] private LayerMask obstacleLayer;

    private void Awake()
    {
        bga = GetComponent<BehaviorGraphAgent>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        isCanSee = CanSeePlayer();
        bga?.SetVariableValue("CanSeePlayer", isCanSee);
        
    }

    private bool CanSeePlayer()
    {
        if(target == null)
            return false;

        float distance = Vector3.Distance(transform.position, target.position);
        return distance <= detectRange && IsInViewAngle() && HasLineOfSight();
    }


    private bool IsInViewAngle()
    {

        // 내 위치에서 타겟으로의 방향
        Vector3 dirToTarget = (target.position - transform.position).normalized;

        // 내 기준으로 앞에 있는지 뒤에 있는지 확인 == 내적
        float dot = Vector3.Dot(transform.forward, dirToTarget);

        // 시야각을 정해주고,
        float threshold = Mathf.Cos(viewAngle * 0.5f * Mathf.Deg2Rad);

        // 시야각 안에 있는지 bool 반환
        return dot >= threshold;
    }

    private bool HasLineOfSight()
    {
        // 내가 레이를 쏘는 위치를 지정해준다 == eyeHeight
        Vector3 eye = transform.position + Vector3.up * eyeHeight;
        Vector3 targetEye = target.position + Vector3.up * eyeHeight;


        //타겟으로의 방향과 타겟까지의 거리를 구해줌
        Vector3 dirToTarget = (targetEye - eye).normalized;
        float distanceTarget = Vector3.Distance(eye, targetEye);

        // 장애물 레이어가 아닐때만 true;
        return !Physics.Raycast(eye, dirToTarget, distanceTarget, obstacleLayer);
    }
}
