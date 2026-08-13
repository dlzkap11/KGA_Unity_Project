using System.Runtime.InteropServices;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class EnemyChaser : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float detectRange;
    //[SerializeField] private float patroDis;
    [SerializeField] private float atkRange;
    [SerializeField] private float giveUpRange;
    [SerializeField] private float rotateSensitivity;
    Vector3 _desPos;

    private NavMeshAgent agent;
    [SerializeField] private bool isLook;


    //공격 관련
    [Header("공격")]
    [SerializeField] private float atkRadius;
    [SerializeField] private float attackForward;
    [SerializeField] private float attackCooldown;
    [SerializeField] private LayerMask attackLayer;
    int i = 0;

    private float attackTimer;

    [Header("감지")]
    [SerializeField] private float viewAngle;
    [SerializeField] private float eyeHeight;
    [SerializeField] private LayerMask obstacleLayer;

    //순찰 관련
    [Header("순찰")]
    [SerializeField] private Transform[] patrolLocation;
    [SerializeField] private float patrolCooldown;
    [SerializeField] private float arriveDis;

    private int currentIndex;
    private bool isStay;
    private float stayTimer;

    
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = atkRange;
        stayTimer = patrolCooldown;
    }

    void Start()
    {
        
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

    private void Patrol()
    {
        if (patrolLocation == null || patrolLocation.Length == 0)
            return;

        // 도착 후 대기시간
        if (isStay)
        {
            stayTimer -= Time.deltaTime;
            agent.isStopped = true;
            if(stayTimer <= 0f)
            {
                isStay = false;
                stayTimer = patrolCooldown;
                currentIndex = (currentIndex + 1) % patrolLocation.Length; // 다음 순찰지점으로 넘기기
            }

            return;
        }

        // 이동
        agent.isStopped = false;
        agent.SetDestination(patrolLocation[currentIndex].position);

        if(!agent.pathPending && agent.remainingDistance <= arriveDis)
            isStay = true;

    }


    void Update()
    {
        if (target == null)
            return;

        float dis = (target.position - transform.position).magnitude;
        float distance = Vector3.Distance(transform.position, target.position);

        // 타겟 - 내 거리가 감지거리보다 작으면
        // 1.아직 감지 전
        // 2.충분히 거리가 가까운 지
        // 3.내 시야 각에 들어와 있는지
        // 4.기둥 뒤에 공간이 있어요
        if (!isLook && dis <= detectRange && IsInViewAngle() && HasLineOfSight())
        {
            Debug.Log("감지!");
            isLook = true;
        }

        if(isLook && dis >= giveUpRange)
        {
            Debug.Log("추적 포기!");
            isLook = false;
            agent.ResetPath();
            return;
             // 원래 상태로 가기
        }

        if (!isLook)
        {
            Patrol();
            return;
        }

        if(dis > atkRange)
        {
            agent.isStopped = false;
            agent.SetDestination(target.position);
        }
        else
        {
            agent.isStopped = true;
            LookAtTarget();
            
            attackTimer -= Time.deltaTime;

            if(attackTimer <= 0f)
            {
                Attack();
                attackTimer = attackCooldown;
                i = 0;
            }

        }
    }

    private void LookAtTarget()
    {
        //보간
        //타깃을 바라보기:  
        //  방향 ← 타깃 위치 − 내 위치, 높이 성분은 0으로      // 고개가 위아래로 꺾이지 않게  
        //  방향이 거의 0이면 → 종료                           // 겹쳤을 때 회전 폭주 방지  
        //  현재 회전에서 목표 회전으로 부드럽게 보간           // 뚝 돌지 않고 스르륵
        Vector3 direction = target.position - transform.position;

        direction.y = 0;

        if (direction.sqrMagnitude < 0.01f)
            return;

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * rotateSensitivity);

    }

    private Vector3 GetForwardTransform()
    {
        return transform.position + transform.forward * attackForward;
    }

    private void Attack()
    {
        // PlayerLayer를 검출한 후 해당 게임 오브젝트의 이름을 가져와서 타격
        //layerMask에 해당하는 collider만 가져온다. 아마도 그럼 layerMask에 장애물이나 플레이어만 등록하면 될 듯 ㄹㅇㄹㅇ
        Collider[] colliders = Physics.OverlapSphere(GetForwardTransform(), atkRadius, attackLayer);
        

        if(colliders.Length == 0)
        {
            Debug.Log("감나빗...");
        }
        else
        {
            foreach (Collider collider in colliders)
            {
                Debug.Log($"{collider.name} 공격! {i}");
                i++;
            }
        }
        
        
        


    }


    private void OnDrawGizmos()
    {

        

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, giveUpRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, atkRange);

        Gizmos.color = Color.purple;
        Gizmos.DrawWireSphere(GetForwardTransform(), atkRadius);


        //시야각
        Vector3 leftBoundary = Quaternion.Euler(0f, -viewAngle - 0.5f, 0f) * transform.forward;
        Vector3 rightBoundary = Quaternion.Euler(0f, viewAngle - 0.5f, 0f) * transform.forward;

        Gizmos.color = Color.black;
        Gizmos.DrawRay(transform.position, leftBoundary * detectRange);
        Gizmos.DrawRay(transform.position, rightBoundary * detectRange);
    }
}
