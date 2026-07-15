using UnityEngine;

public class Move : MonoBehaviour
{

    [SerializeField]
    private Rigidbody rb;
    private Color color;
    public int count;


    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        color = GetComponent<Renderer>().material.color;
    }

    void Start()
    {
        Debug.Log("Mover ready");
    }

    void Update()
    {

        transform.Rotate(new Vector3(0.0f, 0.5f, 0.0f));


        //transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnMouseDown()
    {
        count++;
        rb.AddForce(new Vector3(0f,10f,0f), ForceMode.Impulse);
    }

    private void OnMouseUp()
    {
        
    }

    private void OnMouseEnter()
    {
        GetComponent<Renderer>().material.color = Color.darkBlue;
    }
    private void OnMouseExit()
    {
        GetComponent<Renderer>().material.color = color;
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 150, 50), $"{count}번 클릭!");
        //GUI.Box(new Rect(10, 10, 150, 50), $"{count}번 클릭!");

    }
}

// 마우스 반응 상자
/*
 * 마우스를 올리면 색이 바뀌고, 벗어나면 원래 색으로 돌아온다
 * 가만히 있는 동안 천천히 계속 회전한다
 * 클릭하면 위로 살짝 튀어오른다 (클릭전 까지는 물리영향을 받지않는상태)
 * 화면 한쪽에 클릭한 횟수가 실시간 보이기
 * 
 * 감시타워
 * 타워 오브젝트 하나와 주변에 굴러다니는ㄴ 큐브 여러개
 * 타워를 선택하면 씬뷰에서 범위가 원으로 표시
 * 일정한 주기마다 범위 안에 어떤 오브젝트가 들어와있는지 확인
 * 범위안 오브젝트는 색이 바뀌고 나가면 원래색
 * 범위 안에 몇개가 있는지 화면에 표시
 * 타워를 클릭하면 그 즉시 다시 확인
 * 
 * 상호작용 미니보스
 * 보스 오브젝트 하나, 클릭 담당하는 별도 스크립트
 * 보스는 두 지점 사이를 부드러운 속도변화로 왕복이동
 * 클릭하면 보스가 맞았는지 판정해서 데미지 주기
 * 데미지를 입으면 잠깐 반짝이는 피격효과
 * 체력이 0이되면 사라진다
 */