using UnityEngine;

public class ObstacleMovement : MonoBehaviour
{
    [SerializeField] private Vector3 moveDirection;
    [SerializeField] private float moveDistance;
    [SerializeField] private float moveSpeed;

    private Vector3 startPositon;
    void Start()
    {
        startPositon = transform.position;
    }

    
    void Update()
    {
        float offset = Mathf.PingPong(Time.time * moveSpeed, moveDistance);

        transform.position = startPositon + moveDirection.normalized * offset;
    }
}
