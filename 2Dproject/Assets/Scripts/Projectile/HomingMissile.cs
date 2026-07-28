using UnityEngine;

// 데이터는 그대로 ProjectileData 사용 or 호밍 특화 데이터를 따로 만들 수도 있음
public class HomingMissile : Projectile
{
    [Header("Homing Missile Specific")]
    public float acceleration = 5f;
    public float maxSpeedMultiplier = 2f;

    public override void Initialize(ProjectileData data, Transform owner, Transform target = null)
    {
        base.Initialize(data, owner, target);

        // 호밍 미사일은 무조건 호밍 타겟 필요
        if (target == null)
        {
            Debug.LogWarning("HomingMissile needs a target!", this);
        }
    }

    protected override void UpdateMovement()
    {
        if (target == null)
        {
            // 타겟이 없으면 그냥 직진
            return;
        }

        Vector2 direction = (target.position - transform.position).normalized;
        float currentSpeed = rb.linearVelocity.magnitude;
        float maxSpeed = data.speed * maxSpeedMultiplier;

        // 서서히 가속
        currentSpeed = Mathf.Min(currentSpeed + acceleration * Time.deltaTime, maxSpeed);

        // 호밍: 방향을 부드럽게 변경
        Vector2 currentVel = rb.linearVelocity.normalized;
        Vector2 newVel = Vector2.Lerp(currentVel, direction, data.homingStrength * Time.deltaTime).normalized * currentSpeed;

        rb.linearVelocity = newVel;
    }

    protected override void OnHit(Collider2D other)
    {
        // 호밍 미사일 특화: 폭파 이펙트나 범위 데미지 등 추가 가능
        base.OnHit(other);

        // 예: 항상 한 대 맞으면 터짐 (관통 무시)
        Die();
    }
}