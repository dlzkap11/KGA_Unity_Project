using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyMovement : MonoBehaviour
{

    private enum EnemyState
    {
        Patrol,
        Chase,
        Attack,
    }

    [SerializeField]
    private Transform[] partrolPoint;
    [SerializeField] private float patrolSpeed;
    [SerializeField]
    private float waitTime;

    [SerializeField] private Transform player;

    [SerializeField] private float detectRange;
    [SerializeField] private float chaseSpeed;

    private Animator animator;
    private Rigidbody rb;

    private int currentPatrolIndex;
    [SerializeField]
    float attackcool = 2.0f;
    private bool isWaiting;
    private float waitTimer;

    [SerializeField]private EnemyState currentState;
    [SerializeField] private bool isAttack;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        rb = player.GetComponent<Rigidbody>();
    }

    
    void Update()
    {

        
        float distancePlayer = Vector3.Distance(player.position, transform.position);
        if(distancePlayer <= detectRange && distancePlayer > 1.0f)
        {
            currentState = EnemyState.Chase;
            
        }
        else if(distancePlayer <= 1.0f)
        {
            currentState = EnemyState.Attack;
        }
        else
        {
            currentState = EnemyState.Patrol;
        }
        
        if (isAttack)
        {

            waitTimer += Time.deltaTime;
            if (waitTimer >= attackcool)
            {

                isAttack = false;
                waitTimer = 0;
            }
            return;
        }
        switch (currentState)
        {
            case EnemyState.Patrol:
                Patrol();
                break; 
            case EnemyState.Chase:
                Chase();
                break;
            case EnemyState.Attack:
                Attack();
                break;

        }

        Debug.Log($"현재 상태 :{currentState}");
    }
    

    private void Patrol()
    {
        // 다음 지정 목표까지 이동,
        // 도착하면
        // 정해진 시간만큼 대기
        // 다음 목적지 설정하고, 이동
        // 반복
        // 각 행동에 따른 애니메이션
        if(isWaiting)
        {
            waitTimer += Time.deltaTime;
            animator.SetFloat("MoveSpeed", 0f);

            if(waitTimer >= waitTime)
            {
                isWaiting = false;
                waitTimer = 0;
                currentPatrolIndex = (currentPatrolIndex + 1) % partrolPoint.Length;
            }

            return;
        }
        Transform target = partrolPoint[currentPatrolIndex];

        
        transform.position = Vector3.MoveTowards(transform.position, target.position, patrolSpeed * Time.deltaTime);
        animator.SetFloat("MoveSpeed", 1f);

        float distance = Vector3.Distance(transform.position, target.position);

        Vector3 direction = target.position - transform.position;
        if(direction.magnitude > 0.01f)
        {
            transform.forward = direction.normalized;
        }


        if(distance < 0.1f)
        {
            isWaiting = true;
        }
        //Vector3 dir = (target.position - rb.position).normalized;
        //rb.MovePosition(rb.position + dir * patrolSpeed * Time.deltaTime);

    }

    private void Chase()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.position, chaseSpeed * Time.deltaTime);

        // 플레이어 방향으로 방향전환
        Vector3 direction = player.position - transform.position;
        if(direction.magnitude > 0.3f)



        if (direction.magnitude > 0.01f)
        {
            transform.forward = direction.normalized;
        }
        // 이동하는 애니메이션 출력
        animator.SetFloat("MoveSpeed", 1f);
    }
    
    private void Attack()
    {
        
        
        // 공격모션 출력

        // 공격모션 타이밍에 맞춰서 플레이어 넉백
        isAttack = true;
        animator.SetTrigger("IsAttack");
        rb.AddExplosionForce(7f, transform.position, 3.0f, 0f, ForceMode.Impulse);
        



    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectRange);
    }
}
