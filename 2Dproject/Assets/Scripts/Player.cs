using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField]
    float _speed = 5.0f;
    [SerializeField]
    float _jumpforce = 6.5f;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    [SerializeField] bool _isJumping = false;
    [SerializeField] public Vector2 _inputVec;
    private Color originColor;
    Vector3 offSet;


    protected Rigidbody2D _rigid;
    protected Animator _animator;
    [SerializeField] public SpriteRenderer _sprite;

    [SerializeField] GameObject _fire;

    public InputAction jumpAction;  // 또는 InputActionReference 사용

    void Awake()
    {
        offSet = new Vector3(0.3f, 0f, 0f);
        _animator = GetComponent<Animator>();
        _sprite = GetComponent<SpriteRenderer>();
        _rigid = GetComponent<Rigidbody2D>();
        originColor = _sprite.color;
        jumpAction = GetComponent<PlayerInput>().actions["Jump"];
    }

    [SerializeField] bool _isGrounded;
    [SerializeField] GameObject ground;
    [SerializeField] Transform groundcheck;

    [SerializeField] private float coyoteTime;
    [SerializeField] private float coyoteTimer;
    [SerializeField] private float jumpBufferTime;
    [SerializeField] private float jumpBufferTimer;

    [SerializeField] private PhysicsMaterial2D physicsMaterial;

    [SerializeField] bool _isWall;
    [SerializeField] GameObject Wall;
    [SerializeField] Transform Wallcheck;
    [SerializeField] LayerMask grounded;
    [SerializeField] LayerMask player;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Ground"), true);
        }


        _isGrounded = Physics2D.OverlapCircle(groundcheck.position, 0.2f, ground.layer);
        _isWall = Physics2D.OverlapCircle(Wallcheck.position, 0.2f, Wall.layer);
        if(_isWall)
        {
            
        }

        // 코요테 타임
        if (_isGrounded)
        {
            coyoteTimer = coyoteTime;
        }
        else
        {
            coyoteTimer -= Time.deltaTime;
        }

        // 점프 버퍼
        if (jumpAction.IsPressed())
        {
            jumpBufferTimer = jumpBufferTime;
        }
        else
        {
            jumpBufferTimer -= Time.deltaTime;
        }

        if(coyoteTimer > 0f && jumpBufferTimer > 0f)
        {
            Debug.Log("Jump!");
            _rigid.linearVelocity = new Vector2(_rigid.linearVelocity.x, 3.0f);
            coyoteTimer = 0f;
        }
    }

    void FixedUpdate()
    {
        // 1. 수평 이동만 velocity에 직접 설정 (수직은 물리 엔진에 맡김)
        
        _rigid.linearVelocity = new Vector2(_inputVec.x * _speed, _rigid.linearVelocity.y);

        // 2. 하강 시 낙하 가속
        if (_rigid.linearVelocity.y < 0)
        {
            _rigid.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }
        // 3. 상승 중 점프키 떼면 점프 높이 제한
        else if (_rigid.linearVelocity.y > 0 && !jumpAction.IsPressed())
        {
            _rigid.linearVelocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
        }
    }

    
    public IEnumerator DownJump()
    {
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Ground"), true);
        yield return new WaitForSeconds(0.5f);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Ground"), false);
    }
    void OnJump(InputValue value)
    {
        if(_inputVec.y < 0f && value.isPressed)
        {
            Debug.Log("밑점");
            StartCoroutine(DownJump());
        }
        

        if (value.isPressed && _isGrounded)
        {
            Debug.Log("_isGrounded pressed");
            //_rigid.linearVelocity = new Vector2(_rigid.linearVelocity.x, 3.0f);
        }
        
        
        if (value.isPressed && !_isJumping)
        {
            Debug.Log("_isJumping pressed");
            _isJumping = true;
            //_rigid.AddForce(Vector2.up * _jumpforce, ForceMode2D.Impulse);
        }
        
    }


    void OnMove(InputValue value)
    {
        _inputVec = value.Get<Vector2>();

        if (_inputVec.x > 0)
        {
            _animator.Play("Move");
            _sprite.flipX = false;
        }
        else if (_inputVec.x < 0)
        {
            _animator.Play("Move");
            _sprite.flipX = true;
        }
        else
        {
            _animator.Play("Idle");
        }
    }

    void OnAttack()
    {
        Debug.Log("공격");
        if (_sprite.flipX)
        {
            GameObject go = GameObject.Instantiate(_fire, transform.position - offSet, Quaternion.identity);
        }

        else
        {
            GameObject.Instantiate(_fire, transform.position + offSet, Quaternion.identity);
        }
            
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            _isJumping = false;
        }
    }

}
