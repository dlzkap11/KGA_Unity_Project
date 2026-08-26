using UnityEngine;
using UnityEngine.AI;

public class MonsterController : MonoBehaviour
{
    public enum State { Idle, Patrol, Chase, GiveUp, Attack, Die, SIZE }
    private IState[] states = new IState[(int)State.SIZE];
    [SerializeField] State curState;
    public State CurState => curState;

    public event System.Action<State> OnStateChanged;

    [SerializeField] NavMeshAgent agent;
    [SerializeField] GameObject target;
    [SerializeField] Vector3 startPosition;

    private void Awake()
    {
        states[(int)State.Idle] = new IdleState(this);
        states[(int)State.Patrol] = new PatrolState(this);
        states[(int)State.Chase] = new ChaseState(this);
        states[(int)State.GiveUp] = new GiveUpState(this);
        curState = State.Idle;
        states[(int)curState].Enter();
        OnStateChanged?.Invoke(curState);

        startPosition = transform.position;
    }

    private void Update()
    {
        states[(int)curState].Update();
    }

    public void ChangeState(State nextState)
    {
        OnStateChanged?.Invoke(nextState);
        Debug.Log($"{curState} -> {nextState}");
        states[(int)curState].Exit();
        curState = nextState;
        states[(int)curState].Enter();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            target = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            target = null;
        }
    }

    private class IdleState : IState
    {
        private MonsterController controller;
        private float timer;
        
        public IdleState(MonsterController controller)
        {
            this.controller = controller;
        }

        public void Enter()
        {
            Debug.Log("Idle Enter");
            controller.agent.isStopped = true;
            timer = 3f;
        }

        public void Exit()
        {
            Debug.Log("Idle Exit");
            controller.agent.isStopped = false;
        }

        public void Update()
        {
            timer -= Time.deltaTime;

            if (timer <= 0)
            {
                controller.ChangeState(State.Patrol);
            }
            else if (controller.target != null)
            {
                controller.ChangeState(State.Chase);
            }
        }
    }

    private class PatrolState : IState
    {
        private MonsterController controller;

        private Vector3 targetPosition;

        public PatrolState(MonsterController controller)
        {
            this.controller = controller;
        }

        public void Enter()
        {
            Debug.Log("Patrol Enter");
            targetPosition = new Vector3(Random.Range(-5, 5), 0, Random.Range(-5, 5));
            controller.agent.isStopped = false;
            controller.agent.destination = targetPosition;
        }

        public void Exit()
        {
            Debug.Log("Patrol Exit");
        }

        public void Update()
        {
            if (Vector3.Distance(controller.transform.position, targetPosition) <= 0.1f)
            {
                controller.ChangeState(State.Idle);
            }
            else if (controller.target != null)
            {
                controller.ChangeState(State.Chase);
            }
        }
    }

    private class ChaseState : IState
    {
        private MonsterController controller;
        public ChaseState(MonsterController controller)
        {
            this.controller = controller;
        }

        public void Enter()
        {
            controller.agent.isStopped = false;
        }

        public void Exit()
        {
            
        }

        public void Update()
        {
            if (controller.target != null)
            {
                controller.agent.destination = controller.target.transform.position;
            }

            if (controller.target == null)
            {
                controller.ChangeState(State.GiveUp);
            }
        }
    }

    public class GiveUpState : IState
    {
        private MonsterController controller;

        public GiveUpState(MonsterController controller)
        {
            this.controller = controller;
        }

        public void Enter()
        {
            controller.agent.destination = controller.startPosition;
        }

        public void Exit()
        {
            
        }

        public void Update()
        {
            if (Vector3.Distance(controller.transform.position, controller.startPosition) <= 0.1f)
            {
                controller.ChangeState(State.Idle);
            }
            else if (controller.target != null)
            {
                controller.ChangeState(State.Chase);
            }
        }
    }
}