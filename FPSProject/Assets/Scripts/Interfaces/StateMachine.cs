using UnityEngine;

public class StateMachine
{
    private IState currentState;

    // 상태변환시
    public void ChangeState(IState state)
    {
        currentState?.Exit();

        currentState = state;

        currentState.Enter();
    }

    // 틱마다 실행
    public void Tick()
    {
        currentState?.Tick();
    }
}
