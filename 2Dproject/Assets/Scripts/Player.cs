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
    bool _isJumping = false;
    public Vector2 _inputVec;
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

    void OnJump(InputValue value)
    {

        if (value.isPressed && !_isJumping)
        {
            Debug.Log("Jump key pressed");
            _isJumping = true;
            _rigid.AddForce(Vector2.up * _jumpforce, ForceMode2D.Impulse);
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
