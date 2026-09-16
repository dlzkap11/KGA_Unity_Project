using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

//[GrabEventLogger 설계도]

//필드: grabInteractable(XRGrabInteractable, 캐싱)

//Awake:
//grabInteractable이 비어있으면 → 자기 자신에서 찾아 채운다

//OnEnable:  
//  selectEntered 이벤트에 OnGrabbed를 구독시킨다  
//  selectExited 이벤트에 OnReleased를 구독시킨다

//OnDisable:  
//  selectEntered 이벤트에서 OnGrabbed 구독을 해지한다   (짝을 맞춰서, 반드시)  
//  selectExited 이벤트에서 OnReleased 구독을 해지한다

//OnGrabbed(정보):  
//  정보 안에 담긴 "누가 잡았는지"를 로그로 남긴다

//OnReleased(정보):  
//  놓았다고 로그로 남긴다

public class GrabEventLogger : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
