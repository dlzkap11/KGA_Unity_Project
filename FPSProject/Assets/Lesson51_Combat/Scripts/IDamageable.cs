using UnityEngine;

/// <summary>
/// [51차시 1교시] "피해를 받을 수 있다"는 약속(인터페이스).
///
/// 메서드 서명 하나뿐이지만, 이 약속 덕분에 공격 코드(PlayerShooter·EnemyAttacker)는
/// 상대가 Player인지 Enemy인지 드럼통인지 전혀 몰라도 된다 —
/// "IDamageable 약속이 있는가?"만 물으면 끝.
///
/// 나중에 ExplodingBarrel : MonoBehaviour, IDamageable 같은 새 클래스가 추가돼도
/// 공격 코드는 한 글자도 안 바뀐다. 이것이 인터페이스를 쓰는 이유.
///
/// ※ 인터페이스는 여러 클래스가 참조하는 '약속'이므로 별도 파일로 두는 것이 관례
///   (어느 한 클래스 파일 안에 숨어 있으면 찾기 어렵다 — 1교시 FAQ).
/// </summary>
public interface IDamageable
{
    /// <param name="amount">피해량</param>
    /// <param name="attacker">공격자 (누가 때렸는지 — 킬 크레딧·넉백 방향 등에 쓸 수 있음)</param>
    void TakeDamage(int amount, GameObject attacker);
}
