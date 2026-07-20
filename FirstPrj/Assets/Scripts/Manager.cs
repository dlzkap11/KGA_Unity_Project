using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;

public class Manager : MonoBehaviour
{

    public UnityEngine.Color color;
    void Update()
    {
        //보스를 클릭하면 상호작용
        //보스가 아닌 부분을 클릭하면 return
        //레이트 레이싱
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Ray가 콜라이더에 부딪혔는지 확인
            if (Physics.Raycast(ray, out hit))
            {
                //Physics.Raycast(transform.position, Vector3.forward, out RaycastHit hit, 10f, targetLayer)
                if (hit.collider.gameObject.name != "Boss")
                    return;
                color = hit.collider.GetComponent<Renderer>().material.color;
                Boss boss = hit.collider.GetComponent<Boss>();
                boss.Hp -= 10;
                hit.collider.GetComponent<Renderer>().material.color = UnityEngine.Color.red;

                // 부딪힌 오브젝트의 이름 출력
                Debug.Log("클릭한 오브젝트: " + hit.collider.gameObject.name);

                // 특정 컴포넌트를 가져오거나 함수를 실행할 수도 있습니다
                //예: hit.collider.GetComponent<MyObjectScript>().DoSomething();
            }
        }

        // 코루틴쓰기
        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Ray가 콜라이더에 부딪혔는지 확인
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.name != "Boss")
                    return;
                Boss boss = hit.collider.GetComponent<Boss>();
                hit.collider.GetComponent<Renderer>().material.color = color;
            }
        }
    }
}
