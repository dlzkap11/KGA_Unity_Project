using UnityEngine;


public class Bat : MonoBehaviour
{
    public enum BatState
    {
        Idle,
        Move,
    }


    //private static readonly MoveHash = Animator.StringToHash("Move");
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Transform player;
    [SerializeField] private float detectRange;
    [SerializeField] private float speed;


    private BatState currentState;
    [SerializeField] private Animator animator;
    private bool isMove = false;

    void Start()
    {
        currentState = BatState.Idle;
        
    }


    void Update()
    {
        float distancePlayer = Vector2.Distance(player.position, transform.position);
        if (distancePlayer <= detectRange)
        {
            currentState = BatState.Move;
        }

            switch (currentState)
        {
            case BatState.Idle:
                OnIdle();
                break;
            case BatState.Move:
                OnMove();
                break;
        }
    }

    void OnIdle()
    {
        
    }

    void OnMove()
    {
        isMove = true;
        transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);

        // 플레이어 방향으로 방향전환
        float direction = player.position.x - transform.position.x;

        if (direction != 0)
            spriteRenderer.flipX = direction < 0;

        animator.Play("Move");
        //animator.SetFloat("OnMove", 0.1f);
    }
}
