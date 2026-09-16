using System.Collections;
using UnityEngine;

//필드: door(문 Transform), openHeight(열리는 높이), openDuration(걸리는 시간)
//내부 상태: 진행 중인 코루틴 핸들, 열림 여부, 닫혀 있을 때의 위치

//Awake:  
//  door가 비어있으면 → 자기 자신을 문으로 쓴다  
//  닫힌 위치를 기억해둔다

//Open():  
//  이미 열려 있으면 → 종료                     (가드 절 1)  
//  이미 여는 중(코루틴이 돌고 있음)이면 → 종료   (가드 절 2 - 51차시에서 쓴 그 패턴)  
//  문 열기 코루틴을 시작한다

//문 열기 코루틴:  
//  경과 시간이 openDuration이 될 때까지 반복  
//      경과 시간 비율만큼 시작 위치에서 목표 위치로 서서히 옮긴다  
//      한 프레임 쉰다  
//  열림 상태로 표시하고, 코루틴 핸들을 비운다

public class DoorOpener : MonoBehaviour
{
    [SerializeField] private Transform door;
    [SerializeField] private float openHeight;
    [SerializeField] private float openDuration;

    Coroutine openCoroutine;
    bool isOpen;
    Vector3 closePos;


    private void Awake()
    {
        if(door == null)
            door = GetComponent<Transform>();
        closePos = door.position;
    }


    public void Open()
    {
        if (isOpen)
            return;
        if (openCoroutine != null)
            return;
        openCoroutine = StartCoroutine(OpenDoor());
    }

    public IEnumerator OpenDoor()
    {
        Vector3 targetPos = closePos + Vector3.up * openHeight;

        float elapsedTime = 0f;

        while (elapsedTime < openDuration)
        {
            elapsedTime += Time.deltaTime;

            float t = Mathf.Clamp01(elapsedTime / openDuration);
            door.position = Vector3.Lerp(closePos, targetPos, t);

            yield return null;
        }

        door.position = targetPos;

        isOpen = true;

        openCoroutine = null;
    }
}
