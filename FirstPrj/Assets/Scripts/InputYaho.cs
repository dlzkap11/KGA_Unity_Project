using UnityEngine;

public class InputYaho : MonoBehaviour
{
    // 커스텀축
    //InputManager에 새로운 Axis를 추가
    // 그거로 이동만들어보기
    
    // 두 힘 합치기
    // InputManager에 Wind라는 이름의 Axis를 만들어 특정 키에 -1 ~ +1값 바인딩
    // 플레이어는 WASD로 만든 이동벡터와 Wind축으로 만든 바람 벡터를 더해서 움직이기
    // 바람없이 이동할 때 바람을 최대로 줬을 때 실제 이동방향이 어떻게 휘는감
    // 최종 이동 벡터의 길이를 화면에 표시 두 벡터를 더했을 때 길이

    // 속도 배율 조절기
    // InputManager에 SpeedBoost라는 Button축을 하나 만들기
    // 평소엔 정규화된 벡터로 이동
    // 해당키를 누르면 같은 방향으로 더 큰 배율로 빨라지게
    // 방향은 그대로 길이만 바뀌는걸 화면 표시


    private Rigidbody rb;
    private float speed = 3.0f;
    private float windspeed = 3.0f;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");
        Vector3 moveVector = new Vector3(x, 0, z).normalized * speed;
        rb.MovePosition(rb.position + moveVector * Time.deltaTime);

        if (Input.GetButtonDown("Jump"))
            rb.AddForce(new Vector3(0f, 3f, 0f), ForceMode.Impulse);
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
