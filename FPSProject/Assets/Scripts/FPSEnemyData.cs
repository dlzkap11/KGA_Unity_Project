using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/FPS Enemy Data")]
public class FPSEnemyData : ScriptableObject
{
    [Header("순찰")]
    public float sightRange;
    public float fieldOfView;

    [Header("이동")]
    public float moveSpeed;

    [Header("공격")]
    public int damage;
    public float attackRange;
    public float combatTime;

    [Header("생존")]
    public int maxHp;
}
