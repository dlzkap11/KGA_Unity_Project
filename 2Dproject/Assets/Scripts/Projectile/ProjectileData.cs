using UnityEngine;

[CreateAssetMenu(fileName = "NewProjectileData", menuName = "Game/Projectile Data")]
public class ProjectileData : ScriptableObject
{
    [Header("Basic")]
    public float speed = 10f;
    public float lifeTime = 3f;
    public int damage = 10;
    public LayerMask hitLayers;

    [Header("Motion")]
    public bool isHoming = false;
    public float homingStrength = 5f;      // 호밍 강도
    public bool rotateToVelocity = true;   // 속도 방향으로 회전

    [Header("Penetration")]
    public int maxPenetrateCount = 0;      // 0이면 관통 없음

    [Header("Effects")]
    public GameObject hitEffectPrefab;
    public AudioClip hitSound;
}