using System.Drawing;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UIElements;
using Color = UnityEngine.Color;

public class Boom : MonoBehaviour
{
    /*
     * 
     폭발 지점 만들기
    여러 개의 상자를 흩어놓고 다음을 구현합니다.
    - 클릭한 지점을 기준으로 AddExplosionForce를 발생시켜 주변 상자들을 한꺼번에 날려버린다
    - 폭발 지점에서 가까운 상자일수록 더 세게, 멀리 있는 상자일수록 약하게 날아간다
    - 폭발이 미치는 반경을 Scene 뷰에서 원(구)으로 표시해 눈으로 확인할 수 있다
    - 상자 중 일부는 Unbreakable이라는 별도 Layer로 지정하고, Collision Matrix 또는 코드에서 이 Layer는 폭발력의 영향을 받지 않도록 만든다
     
     */
    public Rigidbody rb;
    public Collider[] colls;
    public Vector3 mousePos;
    AudioSource audioSource;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }


    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            //audioSource.Play();
            // 마우스 클릭 위치를 월드 좌표로 변환
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Debug.Log($"{Camera.main.ScreenToWorldPoint(Input.mousePosition)}");
            mousePos.z = 0; // 3D인 경우 Z값 보정 필요
            float radius = 3.0f; // 감지할 반경

            // 2D 예시: 반경 내에 있는 콜라이더들을 배열로 가져옴
            Collider[] hitColliders = Physics.OverlapSphere(mousePos, radius);
            foreach (var hit in hitColliders)
            {
                Debug.Log("감지된 오브젝트: " + hit.name);
                rb = hit.GetComponent<Rigidbody>();
                if(rb != null)
                    rb.AddExplosionForce(1000f, mousePos, radius, 20f);
            }
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0; // 3D인 경우 Z값 보정 필요
        Gizmos.DrawWireSphere(mousePos, 3.0f);
    }
}
