using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] PlayerInput input;
    [SerializeField] Rigidbody rb;
    [SerializeField] float speed;
    [SerializeField] float jumpPower;
    private Vector3 moveVector;
    Vector2 inputVec;
    public bool isJumped;

    public Vector3 StartPos;


    private void Awake()
    {
        StartPos = transform.position;
    }

    void Start()
    {

    }

    private void FixedUpdate()
    {
        PlayerMove();

    }

    void PlayerMove()
    {
        // 월드 기준 앞/오른쪽 벡터
        Vector3 forward = transform.forward;
        Vector3 right = transform.right;

        // y 축은 ignore (평면 이동)
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        // 입력을 캐릭터 기준 앞/오른쪽으로 변환
        moveVector = (forward * inputVec.y) + (right * inputVec.x);

        // 필요하면 다시 정규화
        if (moveVector.magnitude > 1f)
            moveVector.Normalize();

        rb.MovePosition(rb.position + moveVector * speed * Time.deltaTime);
    }

    public void OnMove(InputValue value)
    {
        inputVec = value.Get<Vector2>();
    }

    public void OnJump()
    {
        if (isJumped)
            return;
        isJumped = true;
        rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
    }

    

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Float"))
            isJumped = false;


    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            transform.position = StartPos;
        }

    }
}
