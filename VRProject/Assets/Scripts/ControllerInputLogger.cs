using UnityEngine;
using UnityEngine.InputSystem;

//[ControllerInputLogger 의사코드]

//필드: triggerAction, gripAction, stickAction (InputActionProperty)  
//      logInterval (기본값 0.2초)  
//      다음로그시각 (내부 변수)

//OnEnable:  
//  trigger / grip / stick 액션을 각각 Enable한다        // 이걸 짝 없이 켜기만 하면 다음 함정의 재료가 된다

//OnDisable:  
//  trigger / grip / stick 액션을 각각 Disable한다       (Enable과 반드시 짝을 맞춘다)

//Update:  
//  지금 시각이 다음로그시각보다 이르면 → 그냥 종료         (가드 절 — 로그 폭주 방지)  
//  다음로그시각 ← 지금 시각 + logInterval

//  trigger값 ← triggerAction에서 float로 읽기  
//  grip값    ← gripAction에서 float로 읽기  
//  stick값   ← stickAction에서 Vector2로 읽기

//  세 값을 로그로 출력한다

public class ControllerInputLogger : MonoBehaviour
{
    [SerializeField] private InputActionProperty triggerAction;
    [SerializeField] private InputActionProperty gripAction;
    [SerializeField] private InputActionProperty stickAction;

    float logInterval = 0.2f;
    float nextLogTime;


    private void OnEnable()
    {
        triggerAction.action.Enable();
        gripAction.action.Enable();
        stickAction.action.Enable();
    }

    private void OnDisable()
    {
        triggerAction.action.Disable();
        gripAction.action.Disable();
        stickAction.action.Disable();
    }

    private void Update()
    {
        if (Time.time <= nextLogTime) return;

        nextLogTime = Time.time + logInterval;

        float trigger = triggerAction.action.ReadValue<float>();
        float grip = gripAction.action.ReadValue<float>();
        Vector2 stick = stickAction.action.ReadValue<Vector2>();

        Debug.Log($"Trigger : {trigger:F2} | Grip : {grip:F2} | Stick : {stick}");
    }

}
