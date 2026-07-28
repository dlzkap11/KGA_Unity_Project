using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    protected ProjectileData data;
    protected Rigidbody2D rb;
    protected Transform owner;      //누가 쐈는지 (플레이어/몬스터)
    protected Transform target;     // 호밍 타겟
    protected int penetrateCount;   // 현재 관통 횟수
    protected bool isDead;

    public virtual void Initialize(ProjectileData data, Transform owner, Transform target = null)
    {
        this.data = data;
        this.owner = owner;
        this.target = target;
        this.penetrateCount = 0;
        this.isDead = false;

        rb = GetComponent<Rigidbody2D>();
        SpriteRenderer sr = this.owner.GetComponent<SpriteRenderer>();
        // 초기 속도 (발사 방향은 transform.right 기준 예시)
        if(sr.flipX)
        {
            rb.linearVelocity = -transform.right * data.speed;
        }
        else
        {
            rb.linearVelocity = transform.right * data.speed;
        }
        

        // 수명
        Destroy(gameObject, data.lifeTime);
    }

    protected virtual void Update()
    {
        if (isDead) return;

        UpdateMovement();
        UpdateRotation();
    }

    protected virtual void UpdateMovement()
    {
        if (!data.isHoming || target == null) return;

        // 기본 호밍 로직
        Vector2 direction = (target.position - transform.position).normalized;
        Vector2 currentVel = rb.linearVelocity;
        Vector2 targetVel = direction * data.speed;

        // 호밍 강도에 따라 부드럽게 전환
        rb.linearVelocity = Vector2.Lerp(currentVel, targetVel, data.homingStrength * Time.deltaTime);
    }

    protected virtual void UpdateRotation()
    {
        if (!data.rotateToVelocity || rb.linearVelocity.sqrMagnitude < 0.01f) return;

        float angle = Mathf.Atan2(rb.linearVelocity.y, rb.linearVelocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    protected virtual void OnHit(Collider2D other)
    {
        if (isDead) return;

        // 자기 자신이나 소유자와는 충돌 무시
        if (other.transform == owner) return;

        // 레이어 체크
        if (((1 << other.gameObject.layer) & data.hitLayers) == 0)
            return;

        // 데미지 처리 (예: 다른 컴포넌트나 인터페이스에 위임 가능)
        
        Player health = other.GetComponent<Player>();
        if (health != null)
        {
            health.TakeDamage();
        }
        
        // 이펙트
        if (data.hitEffectPrefab != null)
        {
            Instantiate(data.hitEffectPrefab, transform.position, Quaternion.identity);
        }

        // 관통 처리
        penetrateCount++;
        if (penetrateCount > data.maxPenetrateCount)
        {
            Die();
        }
        else
        {
            // 관통 시 약간의 이펙트/로직 추가 가능
        }
    }

    protected virtual void Die()
    {
        if (isDead) return;
        isDead = true;

        // 사운드 등
        // if (data.hitSound != null) AudioSource.PlayClipAtPoint(data.hitSound, transform.position);

        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        foreach (var contact in collision.contacts)
        {
            OnHit(contact.otherCollider);
            if (isDead) break;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        OnHit(other);
    }
}