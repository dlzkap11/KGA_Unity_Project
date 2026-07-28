// 키 입력 예
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public Shooter shooter;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
            shooter.TryAttack("A"); // A 투사체

        if (Input.GetKeyDown(KeyCode.V))
            shooter.TryAttack("B"); // B 투사체
    }
}