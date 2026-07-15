using Unity.VisualScripting;
using UnityEditor.AssetImporters;
using UnityEngine;

public class Boss : MonoBehaviour
{/*
    * 
 * 상호작용 미니보스
 *보스 오브젝트 하나, 클릭 담당하는 별도 스크립트
 * 보스는 두 지점 사이를 부드러운 속도변화로 왕복이동
 *클릭하면 보스가 맞았는지 판정해서 데미지 주기
 * 데미지를 입으면 잠깐 반짝이는 피격효과
 *체력이 0이되면 사라진다
 */
    public int Hp;
    public float amplitude = 3.0f; // 움직임의 범위
    public float frequency = 1.0f; // 움직임의 속도
    Color color;
    private Vector3 startPosition;

    private float timer;
    private void Awake()
    {
        Hp = 100;
        startPosition = transform.position;
        color = GetComponent<Renderer>().material.color;
    }

    void Start()
    {
    }

    void Update()
    {
        // 시간에 따라 사인파를 생성하여 위치를 계산
        float newY = startPosition.y + amplitude * Mathf.Sin(Time.time * frequency);
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);

        if(Hp <= 0)
        {
            Destroy(gameObject);
        }
    }
}
