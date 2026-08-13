using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform muzzlePoint;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float bulletLifetime;
    [SerializeField] private float muzzlePointOffset;

    [SerializeField] private ObjectPool bulletPool;

    private SpriteRenderer sr;
    private float direction;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    public void Fire()
    {
        //GameObject bullet = Instantiate(bulletPrefab, muzzlePoint.position, muzzlePoint.rotation);
        GameObject bullet = bulletPool.GetObj();
        bullet.transform.position = muzzlePoint.position;


        SpriteRenderer bulletRenderer = bullet.GetComponent<SpriteRenderer>();
        bulletRenderer.flipX = sr.flipX;

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.linearVelocity = muzzlePoint.right * bulletSpeed * direction;

        StartCoroutine("ReturnBullet", bullet);
        
    }

    private IEnumerator ReturnBullet(GameObject bullet)
    {
        yield return new WaitForSeconds(bulletLifetime);
        bulletPool.Return(bullet);
    }
    
    void Update()
    {

        direction = sr.flipX ? -1f : 1f;
        muzzlePoint.localPosition = new Vector3(muzzlePointOffset * direction, muzzlePoint.localPosition.y, 0f);

        if (Input.GetKeyDown(KeyCode.B))
        {
            Fire();
        }
    }
}
