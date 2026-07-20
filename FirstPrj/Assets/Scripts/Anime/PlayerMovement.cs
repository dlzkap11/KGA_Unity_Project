using UnityEngine;


[RequireComponent(typeof(Animator))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    float moveSpeed;
    [SerializeField]
    float jumpPower;
    bool IsRun;
    bool IsRolling;
    public bool IsGrounded;
    private Animator animator;
    private Rigidbody rb;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    
    void Update()
    {
        if (IsRolling)
            return;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            IsRun = true;
        }
        else
        {
            IsRun = false;
        }

        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");
        Vector3 moveVector = new Vector3(x, 0f, z);
        transform.Translate(moveVector.normalized * moveSpeed * Time.deltaTime);

        //transform.position += moveVector * moveSpeed * Time.deltaTime;

        animator.SetFloat("MoveSpeed", moveVector.magnitude);
        animator.SetBool("IsRun", IsRun);
        animator.SetBool("IsJumping", IsGrounded);

        /*
        if (moveVector.magnitude > 0.01f)
        {
            transform.forward = moveVector.normalized;
        }
        */
        if (Input.GetKeyDown(KeyCode.Space))
        {
            IsGrounded = false;
            animator.SetTrigger("IsJumped");
            rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            IsRolling = true;
            animator.SetTrigger("IsRoll");
            rb.AddForce(Vector3.forward * jumpPower, ForceMode.Impulse);
            
        }
    }

    private void OnCollisionEnter(UnityEngine.Collision collision)
    {
        if(collision.gameObject.CompareTag("Plane"))
        {
            IsGrounded = true;
        }
    }
}
