using UnityEngine;

//[MinimapFollow 의사코드]

//계약: 위치는 플레이어를 따라간다 / 회전은 항상 고정이다(위에서 아래로, top-down)

//필드: 대상(플레이어 Transform), 높이(6번 항목에서 실험으로 정한 값)

//Update:
//카메라 위치 ← (대상의 x, 높이, 대상의 z)     // y만 고정값, x·z는 플레이어를 따라간다  
//카메라 회전은 건드리지 않는다                 // Orthographic + 고정 회전 = 항상 같은 각도로 내려다봄


public class MinimapCameraFollow : MonoBehaviour
{
    [SerializeField] private Transform player;

    void Start()
    {
        
    }



    void LateUpdate()
    {
        transform.position = new Vector3(player.position.x, transform.position.y, player.position.z);
    }
}
