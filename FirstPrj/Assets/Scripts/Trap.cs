using UnityEngine;

public class Trap : MonoBehaviour
{


    /*
     * 오전 실습

    회전하며 날아가는 상자
    상자 여러 개를 만들어 다음을 구현합니다.
    - 클릭하면 그 상자가 Impulse 힘으로 앞을 향해 날아간다
    - 동시에 AddTorque로 회전력도 받아서 빙글빙글 돌면서 날아간다
    - Physic Material을 적용해서 바닥에 닿을 때마다 튀어오른다
    - 상자마다 Mass를 다르게 설정해두고, 같은 힘에도 얼마나 다르게 날아가는지 비교한다
    
    폭발 지점 만들기
    여러 개의 상자를 흩어놓고 다음을 구현합니다.
    - 클릭한 지점을 기준으로 AddExplosionForce를 발생시켜 주변 상자들을 한꺼번에 날려버린다
    - 폭발 지점에서 가까운 상자일수록 더 세게, 멀리 있는 상자일수록 약하게 날아간다
    - 폭발이 미치는 반경을 Scene 뷰에서 원(구)으로 표시해 눈으로 확인할 수 있다
    - 상자 중 일부는 Unbreakable이라는 별도 Layer로 지정하고, Collision Matrix 또는 코드에서 이 Layer는 폭발력의 영향을 받지 않도록 만든다
    
    장애물을 감지하고 밀어내는 정찰 드론
    드론 오브젝트와 장애물 여러 개를 만들어 다음을 구현합니다.
    - AddForce 대신 velocity를 직접 지정해서, 드론이 일정한 속도로 계속 앞으로 이동한다
    - 이동 중 전방으로 Raycast를 쏴서 장애물을 감지한다 (LayerMask로 Obstacle Layer만 감지하도록 필터링)
    - 장애물이 감지되면 드론의 velocity를 0으로 멈추고, 그 자리에서 AddExplosionForce로 장애물을 밀어낸다
    - 장애물이 치워지면 드론이 다시 원래 속도로 이동을 재개한다
    - 드론과 장애물은 서로 다른 Layer로 분리되어 있고, Collision Matrix에서 드론과 장애물만 충돌하도록 설정되어 있다 (다른 드론끼리는 서로 무시)
    */
    Rigidbody rb;
    public float impulsePower = 4.0f;
    public float speed = 4.0f;
    public float expolosionPower = 100.0f;
    public float radios = 100.0f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnMouseDown()
    {
        rb.AddForce(Vector3.back * impulsePower, ForceMode.Impulse);
        rb.AddTorque(Vector3.back * impulsePower, ForceMode.Impulse);
    }

    private void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");
        Vector3 moveVector = new Vector3(x, 0, z).normalized * speed;
        rb.MovePosition(rb.position + moveVector * Time.deltaTime);


        if (Input.GetKeyDown(KeyCode.Z))
        {
            Debug.Log("z");
            rb.AddForce(Vector3.up * impulsePower, ForceMode.Impulse);
        }


        if (Input.GetKey(KeyCode.X))
        {
            Debug.Log("x");
            rb.AddForce(Vector3.up * impulsePower, ForceMode.Force);
        }

        if (Input.GetKey(KeyCode.C))
        {
            Debug.Log("c");
            rb.linearVelocity = Vector3.zero;
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            Debug.Log("B");
            rb.AddExplosionForce(100f, Vector3.zero, radios, 20f);
            //rb.AddExplosionForce(expolosionPower, rb.position, radios);
        }
    }
}
