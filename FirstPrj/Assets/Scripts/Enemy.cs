using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float speed = 2.0f;
    private float distance = 3.0f;
    public GameObject target;
    private Rigidbody rb;
    public State state;
    Vector3 startPos;
    Vector3 dir;

    //적 Enum으로 상태를 만들기
    public enum State
    {
        Idle,
        Patrol,
        Back,
        Tracking,
    }

    private void Awake()
    {
        state = State.Patrol;
        rb = GetComponent<Rigidbody>();
        startPos = new Vector3(transform.position.x, -0.5f, transform.position.z);
    }

    private void FixedUpdate()
    {
        if (state == State.Patrol)
        {
            Vector3 way = new Vector3(3.659f, -0.5f, 1.9f);
            float tt = (way - rb.position).magnitude;
            if(tt < 0.5f)
            {
                state = State.Back;
                return;
            }
            dir = (way - rb.position).normalized;
            rb.MovePosition(rb.position + dir * speed * Time.deltaTime);
        }
        else if (state == State.Back)
        {
            float distance = (startPos - transform.position).magnitude;
            if (distance < 0.5f)
            {
                state = State.Patrol;
                return;
            }

            Vector3 back = (startPos - transform.position).normalized;
            rb.MovePosition(rb.position + back * speed * Time.deltaTime);
        }
        else if (state == State.Tracking)
        {
            float distance = (target.transform.position - rb.position).magnitude;
            dir = (target.transform.position - rb.position).normalized;
            rb.MovePosition(rb.position + dir * speed * Time.deltaTime);
        }
        else
            return;

        /*
        if (target == null)
        {
            
            if (distance < 0.1f)
            {
                float tt = (target.transform.position - rb.position).magnitude;
                while (tt > 0.1f)
                {
                    dir = (target.transform.position - rb.position).normalized;
                    rb.MovePosition(rb.position + dir * speed * Time.deltaTime);
                }
                
            }
            else
            {
                Vector3 back = (startPos - transform.position).normalized;
                rb.MovePosition(rb.position + back * speed * Time.deltaTime);
            }
            
            
        }
        else
        {
            //내 위치와 타겟위치를 보고 갈 방향을 정해야함
            float distance = (target.transform.position - rb.position).magnitude;
            dir = (target.transform.position - rb.position).normalized;
            rb.MovePosition(rb.position + dir * speed * Time.deltaTime);
        }
        */
    }

    private void OnCollisionEnter(UnityEngine.Collision collision)
    {
        if (collision.gameObject.tag != "Player")
            return;

        Debug.Log("속이 뻥!");
        dir = (target.transform.position - rb.position).normalized;
        collision.rigidbody.AddForce((Vector3.up + dir) * 50.0f, ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player")
            return;
        state = State.Tracking;
        target = (GameObject)other.gameObject;

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag != "Player")
            return;
        state = State.Back;
        target = null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 2.0f);
        //Gizmos.DrawSphere(transform.position, 3.0f);

    }
}
