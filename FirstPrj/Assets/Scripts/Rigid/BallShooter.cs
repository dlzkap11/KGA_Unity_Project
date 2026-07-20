using UnityEngine;

public class BallShooter : MonoBehaviour
{
    [SerializeField] Ball ballPrefab;
    [SerializeField] Transform spawnPoint;
    [SerializeField] float impulsePower = 18f;
    [SerializeField] float torquePower = 8f;
    [SerializeField] float minMass = 0.5f;
    [SerializeField] float maxMass = 3f;
    [SerializeField] float lifeTime = 8f;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SpawnBall();
        }
    }

    void SpawnBall()
    {
        //카메라 기준으로 했을 때 
        //Ray를 보면
        var ball = Instantiate(ballPrefab, spawnPoint.position, Quaternion.identity);

        float mass = Random.Range(minMass, maxMass);
        ball.Init(mass, lifeTime);

        Vector3 dir = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        dir.z = spawnPoint.position.z;
        Vector3 forceDir = (dir - spawnPoint.position).normalized;

        ball.Launch(forceDir, impulsePower, torquePower);
    }
}