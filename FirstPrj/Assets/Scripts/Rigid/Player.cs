using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{

    int rayCount = 10; // 쏠 레이저 개수
    float angleRange = 45f; // 부채꼴 범위 (각도)

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

    private void Update()
    {
        

        for (int i = 0; i < rayCount; i++)
        {
            float angle = transform.eulerAngles.y - angleRange / 2 + (angleRange / rayCount * i);
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.up);
            Vector3 direction = rotation * Vector3.forward;
            
            RaycastHit hit;
            if (Physics.Raycast(transform.position, direction, out hit, 10.0f))
            {
                Debug.Log(hit.collider.name + " 감지");
                
            }
        }
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
}
