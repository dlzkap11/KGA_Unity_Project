using System.Xml.Schema;
using UnityEngine;

/// <summary>
/// [52차시 7교시] 히트스캔의 FPS 전환 — 51차시 PlayerShooter에서 고친 것은 "딱 한 줄"이다.
///
///   51차시(쿼터뷰): Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
///   52차시(1인칭):  Ray ray = new Ray(fpsCamera.transform.position, fpsCamera.transform.forward);
///
/// 커서가 잠겨 있으니 조준점은 항상 화면 중앙 = 카메라 정면. 화면 좌표가 필요 없어져서
/// 광선의 시작점과 방향을 직접 만들면 끝. 나머지(Raycast → IDamageable → TakeDamage)는 그대로다.
///
/// ★ 나흘간의 성적표: 쿼터뷰→1인칭 전환에 PlayerShooter 1줄, Health·IDamageable·HitFlash·
///   HitSound·KillCounter 전부 0줄. "부품으로 나눠 만들었으니 프로젝트가 바뀌어도 살아남는다."
///
/// ※ IDamageable·Health는 51차시 패키지의 부품을 그대로 재사용 — Lesson51 임포트 필요.
/// </summary>
public class FpsShooter : MonoBehaviour
{
    [SerializeField] AmmoSystem ammo;
    [SerializeField] ParticleSystem muzzleFlash;
    [SerializeField] ParticleSystem baseHitEffect;
    [Header("사격 설정")]
    [SerializeField] private Camera fpsCamera;        // 1인칭 카메라 (비워두면 Camera.main)
    [SerializeField] private int damage = 10;
    [SerializeField] private float maxDistance = 100f;
    [SerializeField] private LayerMask hitMask = ~0;  // Player 자신의 레이어는 제외

    [Header("발사 연출 (7교시 선택 — 무기 Animator에 Fire 트리거)")]
    [SerializeField] private Animator weaponAnimator; // 비워두면 연출 없이 동작 (Unity 오브젝트라 ?.가 아닌 if로 가드!)

    private void Awake()
    {
        if (fpsCamera == null)
        {
            fpsCamera = Camera.main;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ammo.Reloding();
        }



        if (!Input.GetMouseButtonDown(0))
        {
            return;
        }
        ammo.Fire();
        muzzleFlash.Play();

        // ★ 오늘 고친 딱 한 줄 — 화면 클릭 좌표 대신 "카메라 정면" 광선
        //   (51차시: fpsCamera.ScreenPointToRay(Input.mousePosition))
        Ray ray = new Ray(fpsCamera.transform.position, fpsCamera.transform.forward);

        // 여기서부터는 51차시와 한 글자도 다르지 않다
        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, hitMask))
        {
            

            HitEffect hitEffect = hit.collider.GetComponent<HitEffect>();
            if(hitEffect != null)
            {
                hitEffect.TakeHit(hit.point, hit.normal);
            }
            else
            {
                ParticleSystem hits = Instantiate(baseHitEffect, hit.point, Quaternion.identity);
                hits.transform.forward = hit.normal;
            }

            IDamageable damageable = hit.collider.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(damage, gameObject);
            }
        }

        // 발사 연출 — 51차시 HitFlash처럼 이벤트 수신기로 뺄 수도 있다 (실습 레벨 2 가산점 포인트)
        if (weaponAnimator != null)
        {
            weaponAnimator.SetTrigger("Fire");
        }
    }
}
