using System;
using UnityEngine;
using UnityEngine.AI;

public class MonsterController : MonoBehaviour
{
    public enum State
    {
        Idle,
        Patrol,
        Chase,
        Giveup,
        Attack,
        Die,
        SIZE // enum 전체 크기
    }

    private IState[] states = new IState[(int)State.SIZE];
    [SerializeField] private State currentState;

    public event Action<State> OnStateChaged;

    [SerializeField] NavMeshAgent m_Agent;

    private void Awake()
    {
        states[(int)State.Idle] = new IdleState(this);
        states[(int)State.Patrol] = new PatrolState(this);
        currentState = State.Idle;
    }

    private void Update()
    {
        states[(int)currentState]?.Tick();
    }

    public void ChangeState(State nextState)
    {
        OnStateChaged?.Invoke(nextState);
        states[(int)currentState]?.Exit();
        currentState = nextState;
        states[(int)currentState]?.Enter();
    }


    private class IdleState : IState
    {
        private MonsterController m_Controller;

        private float timer;

        public IdleState(MonsterController controller)
        {
            m_Controller = controller;
        }

        public void Enter()
        {
            m_Controller.m_Agent.isStopped = true;
            timer = 3f;
        }

        public void Exit()
        {
            m_Controller.m_Agent.isStopped = false;
        }

        public void Tick()
        {
            timer -= Time.deltaTime;

            if (timer <= 0)
            {
                m_Controller.ChangeState(State.Patrol);
            }
        }
    }


    private class PatrolState : IState
    {
        private MonsterController m_Controller;

        private Vector3 targetPos;

        public PatrolState(MonsterController controller)
        {
            m_Controller = controller;
        }


        public void Enter()
        {
            m_Controller.m_Agent.isStopped = false;
            targetPos = new Vector3(UnityEngine.Random.Range(-5, 5), 0, UnityEngine.Random.Range(-5, 5));
            m_Controller.m_Agent.SetDestination(targetPos);
        }

        public void Exit()
        {

        }

        public void Tick()
        {
            if (Vector3.Distance(m_Controller.transform.position, targetPos) <= 0.1f) 
            {
                m_Controller.ChangeState(State.Idle);
            }
        }
    }
}
