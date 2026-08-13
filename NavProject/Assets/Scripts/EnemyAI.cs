using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float detectionRange;
    [SerializeField] private float attackRange;
    [SerializeField] private float giveUpRange;
    [SerializeField] private float rotateSensitivity;

    [Header("공격")]
    [SerializeField] private float attackRadius;
    [SerializeField] private float attackForward;
    [SerializeField] private float attackCooldown;
    [SerializeField] private LayerMask attackLayer;

    [Header("순찰")]
    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private float patrolWaitingTime;
    [SerializeField] private float arrivalThreshold;
    private int currentPatrolIndex;
    private float patrolWaitTimer;
    private bool isWaitingAtPoint;


    private float attackTimer;
    private NavMeshAgent agent;
    private bool hasDetectionTarget;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = attackRange;
        // attackTimer = attackCooldown;
    }

    private void Update()
    {
        if (target == null)
            return;

        // 다만 이렇게 했었을때는, 벽이 있던 없던 거리만 좁혀지면 인식한다.
        float distance = Vector3.Distance(transform.position, target.position);

        // 감지
        if (!hasDetectionTarget && distance <= detectionRange)
        {
            hasDetectionTarget = true;
        }

        // 포기
        if (hasDetectionTarget && distance >= giveUpRange)
        {
            hasDetectionTarget = false;
            agent.ResetPath();
            return;
        }

        // 대기
        if (!hasDetectionTarget)
        {
            Patrol();
            return;
        }

        if (distance > attackRange)
        {
            // 추적
            agent.isStopped = false;
            agent.SetDestination(target.position);
        }
        else
        {
            // 공격
            agent.isStopped = true;
            LookAtTarget();
            // 쿨다운이 지났을때 공격
            attackTimer -= Time.deltaTime;

            if (attackTimer <= 0)
            {
                Attack();
                attackTimer = attackCooldown;
            }
        }

    }

    private void Patrol()
    {
        if (patrolPoints == null || patrolPoints.Length == 0)
            return;

        // 도착해서 대기중일때
        if (isWaitingAtPoint)
        {
            patrolWaitTimer += Time.deltaTime;
            agent.isStopped = true;
            Debug.Log("도착 완료");
            if (patrolWaitingTime <= patrolWaitTimer)
            {
                isWaitingAtPoint = false;
                patrolWaitTimer = 0;

                currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
            }

            return;
        }
        Debug.Log("이동중");

        // 이동중일때
        agent.isStopped = false;
        agent.SetDestination(patrolPoints[currentPatrolIndex].position);

        if (!agent.pathPending && agent.remainingDistance <= arrivalThreshold)
        {
            isWaitingAtPoint = true;
        }
    }

    private Vector3 GetForwardTransform()
    {
        return transform.position + transform.forward * attackForward;
    }

    private void Attack()
    {
        // 내 주변에 주변 콜라이더를 감지는 구체를 그리고. 거기에서 플레이어(공격대상을 가지고 온다)
        Collider[] hits = Physics.OverlapSphere(GetForwardTransform(), attackRadius, attackLayer);

        // 만약 공격하는데 내가 지정해둔 위치에 타겟이 없다면
        if (hits.Length == 0)
            Debug.Log("헛스윙");

        foreach (Collider hit in hits)
        {
            Debug.Log($"{hit.name} 타격");
        }
    }

    private void LookAtTarget()
    {
        Vector3 direction = target.position - transform.position;

        direction.y = 0;

        if (direction.sqrMagnitude < 0.01f)
        {
            return;
        }

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * rotateSensitivity);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(GetForwardTransform(), attackRadius);
    }

}