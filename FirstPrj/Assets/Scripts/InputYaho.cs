using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AdaptivePerformance.Provider;
using UnityEngine.InputSystem;

public class InputYaho : MonoBehaviour
{
    /*
    *   방향 감지 상자
        큐브 하나를 만들어 다음을 구현합니다.
    
        WASD로 이동한다
        대각선으로 움직여도 속도가 더 빨라지지 않는다
        벽에 부딪히면 잠깐 색이 바뀌었다가 원래대로 돌아온다
        
        아이템 수집 지대
        플레이어 하나와 여러 종류의 아이템을 만들어 다음을 구현합니다.
        
        Coin, Heart, Trap 세 종류의 아이템이 씬에 흩어져 있다
        Coin에 닿으면 사라지고 획득 개수가 늘어난다
        Heart에 닿으면 사라지고 체력이 회복된다
        Trap에 닿으면 체력이 1 깎이고, 플레이어가 시작 위치로 되돌아간다
        체력이 0이되면 더 이상 움직이지 않는다.
        현재까지 모은 코인 개수와 체력이 화면에 표시된다
        각 아이템 종류를 색이 다른 표시로 구분해서 볼 수 있다
        
         
        고급: 추격하는 경비병
        경비병 오브젝트 하나와 플레이어를 만들어 다음을 구현합니다.
        
        평소에는 두 지점 사이를 부드럽게 왕복하며 순찰한다
        플레이어가 일정 범위 안에 들어오면 순찰을 멈추고 플레이어 쪽으로 쫓아온다
        플레이어가 범위를 벗어나면 다시 원래 순찰 경로로 돌아간다
        경비병에게 부딪히면 플레이어가 살짝 밀려나고 체력이 줄어든다
        감지 범위는 Scene 뷰에서 원으로 표시되어 눈으로 확인할 수 있다
    */

    private Rigidbody rb;
    private float speed = 3.0f;
    private Color myColor;

    public int MaxHp;

    [SerializeField]
    private int hp;

    public int Hp
    {
        get => hp;
        set
        {
            hp = Mathf.Clamp(value, 0, MaxHp);
        }
    }

    [SerializeField]
    private int coinCnt;

    private bool IsDead => hp <= 0;

    private Vector2 moveVector;
    private Vector3 startPos;
    private void Awake()
    {
        MaxHp = 5;
        Hp = MaxHp;
        rb = GetComponent<Rigidbody>();
        myColor = GetComponent<Renderer>().material.color;
        startPos = transform.position;
    }

    void Start()
    {
        
    }

    void OnMove(InputValue value)
    {
        moveVector = value.Get<Vector2>();
        Debug.Log($"{moveVector} 무빙맨");
    }

    private void FixedUpdate()
    {
        if (IsDead)
            return;

        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");
        Vector3 moveVector = new Vector3(x, 0, z).normalized * speed;
        rb.MovePosition(rb.position + moveVector * Time.deltaTime);

        if (Input.GetButtonDown("Jump"))
            rb.AddForce(new Vector3(0f, 3f, 0f), ForceMode.Impulse);
    }

    private void OnCollisionEnter(UnityEngine.Collision collision)
    {
        if (collision.gameObject.CompareTag("Plane"))
            return;

        GetComponent<Renderer>().material.color = Color.red;

        //코루틴?
        //GetComponent<Renderer>().material.color = myColor;
    }

    private void OnCollisionExit(UnityEngine.Collision collision)
    {
        if (collision.gameObject.CompareTag("Plane"))
            return;
        GetComponent<Renderer>().material.color = myColor;
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.name)
        {
            case "Heart":
                Hp += 1;
                break;
            case "Coin":
                coinCnt++;
                break;
            case "Trap":
                Hp -= 1;
                transform.position = startPos;
                break;
        }
    }


    /*
    void Update()
    {
        float wx = Input.GetAxisRaw("Wind");
        float boost = Input.GetAxisRaw("SpeedBoost");
        
        float x = Input.GetAxisRaw("MyHorizontal");
        float z = Input.GetAxisRaw("MyVertical");
        Debug.Log($"{x}, {z}, {wx}");
        Vector3 windVector = new Vector3(wx, 0, 0) * windspeed;
        Vector3 moveVector = new Vector3(x, 0, z).normalized * speed;
        Debug.Log($"정규화 전 : {moveVector.magnitude}");
        Debug.Log($"정규화 후: {moveVector.normalized.magnitude}");

        Debug.Log($"wind정규화 후: {(windVector + moveVector).normalized.magnitude}");
        Debug.Log($"windnormal정규화 후: {(windVector.normalized + moveVector.normalized).magnitude}");
        
        //transform.position += moveVector * speed * Time.deltaTime;


        if (Input.GetButtonDown("SpeedBoost"))
        {
            transform.Translate((windVector + moveVector) * boost * Time.deltaTime);
        }
        else
        {
            transform.Translate((windVector + moveVector) * Time.deltaTime);
        }



        if (Input.GetButtonDown("Jump"))
            rb.AddForce(new Vector3(0f, 3f, 0f), ForceMode.Impulse);
        

    }
    */
}
