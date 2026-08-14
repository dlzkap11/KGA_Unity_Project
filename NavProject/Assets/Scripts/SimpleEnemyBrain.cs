using Unity.VisualScripting;
using UnityEditor.Searcher;
using UnityEngine;

//    상태 목록(enum): Patrol, Chase, Attack, Search
public enum State
{
    Patrol,
    Chase,
    Attack,
    Search
}

public class SimpleEnemyBrain : MonoBehaviour
{
    //    [SimpleEnemyBrain 의사코드]
    //    필드: 타깃, 시야센서(5절의 그 센서 — 눈은 하나, 뇌는 둘), 공격거리
    [SerializeField] private Transform target;
    [SerializeField] private CanSee sight;
    [SerializeField] private float atkRange;
    [SerializeField] private State currnetState = State.Patrol;

    private Vector3 lastKnowPosition;

    //    Update:  
    //  보인다 ← 센서가 공개한 판정(센서가 없으면 안 보임으로)
    //  거리 ← 타깃이 없으면 무한대, 있으면 타깃까지 거리    // 타깃이 없어도 에러 없이 순찰 유지  
    //  [공격] 보인다 && 거리 ≤ 공격거리 → 현재상태 ← Attack
    //  [추적] 아니고, 보인다 → 현재상태 ← Chase, 마지막목격지점 ← 타깃 위치(매 프레임 갱신)
    //  [수색] 아니고, 마지막목격지점이 기록돼 있으면 → 현재상태 ← Search
    //  [순찰] 아니면 → 현재상태 ← Patrol
    //  상태 처리 실행(지금은 현재 상태를 로그로만)

    //※ 대괄호 순서 = 6절 설계표의 위→아래 순서 = else if 사슬의 순서.
    //  Attack이 Chase보다 위에 있는 이유는 이미 설계표에서 정했다.위에 쓴 if가 이긴다.

    void Start()
    {
        
    }

    bool canSeeTarget;
    void Update()
    {

        
        if (sight != null)
            canSeeTarget = sight.isCanSee;

        
        float distance;
        if (target != null)
            distance = Vector3.Distance(transform.position, target.position);
        else
            distance = float.MaxValue;
        

        if(canSeeTarget && distance <= atkRange)
        {
            currnetState = State.Attack;
        }
        else if (canSeeTarget)
        {
            currnetState = State.Chase;
        }
        else if(lastKnowPosition != Vector3.zero)
        {
            currnetState = State.Search;
        }
        else
        {
            currnetState = State.Patrol;
        }
    }
}
