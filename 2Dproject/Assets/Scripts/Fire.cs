using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Fire : MonoBehaviour
{
    GameObject go;
    [SerializeField] Player _player;
    [SerializeField] protected SpriteRenderer _sprite;
    float speed;
    bool _flip;

    private void Awake()
    {
        speed = 1.0f;
    }

    void Start()
    {
        go = GameObject.FindWithTag("Player");
        _player = go.GetComponent<Player>();
        _flip = _player._sprite.flipX;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            return;
        Destroy(gameObject);
    }

    void Update()
    {
        if (_flip)
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime);
        }
        else
        {
            transform.Translate(-Vector3.left * speed * Time.deltaTime);
        }
    }
}
