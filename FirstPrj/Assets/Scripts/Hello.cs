using Unity.VisualScripting;
using UnityEngine;

public class Hello : MonoBehaviour
{

    [Header("야호")]

    [SerializeField]
    private Rigidbody rb;

    [SerializeField]
    private Rigidbody[] rb2;

    [HideInInspector]
    public GameObject obj;
    [Space(10)]
    public Color color;
    [Range(0f, 10f)]
    public int count;

    private void Awake()
    {
        // 해당 컴포넌트가 생성되고 한번 - 나 자신의 초기화에 많이 씀
        // 자기자신도 포함
        rb = GetComponentInChildren<Rigidbody>(); // 자식한테 받아오기
        rb = GetComponentInParent<Rigidbody>(); //부모한테 받아오기

        rb2 = GetComponentsInChildren<Rigidbody>(); //여러개 받아오기 (첫빠따는 자기자신것을 가져옴-없으면 패스)
        color = GetComponent<Renderer>().material.color;
        Debug.Log("Awake");
    }

    private void OnEnable()
    {
        // 컴포넌트가 활성화 되고 한번 - 이벤트 구독에 많이씀
        
        Debug.Log("Enable");
    }

    private void Start()
    {
        // 컴포넌트 활성화 이후 첫번째 업데이트 이전에 한번 - 외부 초기화 가능
        Debug.Log("Start");
    }
    private void FixedUpdate()
    {
        // 내부 타이머를 통해 물리엔진 연산을 일정하게 돌림
        Debug.Log("Fixed");
    }

    void Update()
    {
        // 매 프레임 호출
        Debug.Log("Update");
        if (count >= 10)
        {
            Destroy(gameObject);
        }

        //Instantiate(obj);
    }


    private void LateUpdate()
    {
        // Update 이후 한번 - 카메라 움직임
        Debug.Log("Late");
    }

    private void OnDisable()
    {
        // 컴포넌트 비활성화시 한번 - 이벤트 해제
        Debug.Log("Disable");
    }

    private void OnDestroy()
    {
        // 컴포넌트 파괴시
        Debug.Log("사라바다");
    }
    
    // 해당 큐브를 클릭하면 색이 바뀜
    // 커서가 벗어나면 원래 색
    // 큐브를 클릭하면 콘솔에 클릭회수가 누적되어 출력
    // 씬뷰에 사거리 표시
    // 씬뷰에서 그 오브젝트를 선택했을 때만 반경 3만큼의 원이 시각적 표시
    // play안해도

    // play중 화면 좌상단
    // 현재까지 출력한 횟수

    // 출력횟수가 10번이면 오브젝트가 스스로 destroy 
    private void OnMouseDown()
    {
        count++;
        Debug.Log($"{count}번 클릭");
        //GetComponent<Renderer>().material.color = Color.red;
        GetComponent<Renderer>().material.color = Random.ColorHSV();
    }

    private void OnMouseDrag()
    {
        Debug.Log("Drag");
    }
    private void OnMouseUp()
    {
        Debug.Log("Up");
    }

    private void OnMouseEnter()
    {
        Debug.Log("Enter");
    }

    private void OnMouseExit()
    {
        
        Debug.Log("Exit");
        GetComponent<Renderer>().material.color = color;


    }

    private void OnMouseOver()
    {
        Debug.Log("Over");
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 3.0f);
        //Gizmos.DrawSphere(transform.position, 3.0f);

    }
    private void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 150, 50), $"{count}번 클릭!");
        //GUI.Box(new Rect(10, 10, 150, 50), $"{count}번 클릭!");

    }
}