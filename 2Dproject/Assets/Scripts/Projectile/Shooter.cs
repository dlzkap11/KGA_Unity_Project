using UnityEngine;

public class Shooter : MonoBehaviour
{
    [System.Serializable]
    public struct AttackInfo
    {
        public string name;           // "A", "B" 등
        public ProjectileData data;
        public GameObject projectilePrefab; // Projectile 또는 서브클래스가 붙은 프리팹
        public Transform firePoint;
        public float cooldown;
    }

    public AttackInfo[] attacks;

    private float[] lastFireTimes;

    private void Start()
    {
        lastFireTimes = new float[attacks.Length];
    }

    public void TryAttack(string attackName)
    {
        for (int i = 0; i < attacks.Length; i++)
        {
            if (attacks[i].name != attackName) continue;

            if (Time.time - lastFireTimes[i] < attacks[i].cooldown)
                return;

            FireAttack(i);
            lastFireTimes[i] = Time.time;
            break;
        }
    }

    private void FireAttack(int index)
    {
        var info = attacks[index];

        if (info.projectilePrefab == null || info.data == null)
        {
            Debug.LogWarning("Projectile or Data is missing for attack: " + info.name, this);
            return;
        }

        Transform fp = info.firePoint != null ? info.firePoint : transform;
        Vector3 pos = new Vector3(fp.position.x, fp.position.y + 0.5f, fp.position.z);
        GameObject obj = Instantiate(info.projectilePrefab, pos, fp.rotation);
        Projectile proj = obj.GetComponent<Projectile>();

        if (proj == null)
        {
            Debug.LogWarning("Projectile component not found on prefab: " + info.projectilePrefab.name, this);
            Destroy(obj);
            return;
        }

        // 타겟은 필요할 때만 (예: 호밍)
        Transform target = FindHomingTarget();

        proj.Initialize(info.data, transform, target);
    }

    private Transform FindHomingTarget()
    {
        // 예: 가장 가까운 적
        // 실제론 레이어, 태그, 컴포넌트 등으로 검색
        return null;
    }
}