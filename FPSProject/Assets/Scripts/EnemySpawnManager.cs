using System.Collections;
using UnityEngine;
using UnityEngine.Events;
//[EnemySpawnManager 의사코드]

//필드: 적 프리팹, 스폰지점 배열, 웨이브당 적 수(3), 소환 간격(0.5초)  
//공개: "적이 소환됐다" 방송 — 소환된 적을 실어서 쏜다    ← 6절 BattleManager가 구독할 계약  
//공개: 웨이브 키우기(양)                                 ← 다음 웨이브를 더 크게 만드는 문

//웨이브 시작:  
//  "웨이브 소환" 절차를 코루틴으로 시작한다               // 한꺼번에 쏟지 않기 위해

//웨이브 소환 (코루틴):  
//  웨이브당 적 수만큼 반복(i):  
//    지점 ← 스폰지점 배열에서 (i를 배열 길이로 나눈 나머지)번째    // 지점 3개, 적 5명이어도 순환  
//    적 ← 프리팹을 그 지점의 위치·회전으로 생성  
//    "적이 소환됐다" 방송을 쏜다(적을 실어서)              // 51차시 OnDamaged와 같은 발행·구독 원리  
//    소환 간격만큼 기다린다                               // 51차시 코루틴 그대로



public class EnemySpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private float spawnDelayTime;

    public UnityEvent<GameObject> OnSpawned;
    public int waveAmount;

    public GameObject player;

    public void StartWave()
    {
        StartCoroutine(SpawnWave());
    }
    
    public IEnumerator SpawnWave()
    {
        for(int i = 0; i < waveAmount; i++)
        {
            GameObject enemy;
            // 순환
            Transform point = spawnPoints[i % spawnPoints.Length];
            enemy = Instantiate(enemyPrefab, point.position, point.rotation);
            FpsEnemyBrain enemyBrain = enemy.GetComponent<FpsEnemyBrain>();
            enemyBrain.SetTarget(player.transform);
            enemyBrain.SetPatrolPoints(spawnPoints[Random.Range(0, spawnPoints.Length - 1)], spawnPoints[Random.Range(0, spawnPoints.Length - 1)], spawnPoints[Random.Range(0, spawnPoints.Length - 1)]);
            // 랜덤
            //enemy = Instantiate(enemyPrefab);
            //enemy.transform.position = spawnPoints[Random.Range(0, spawnPoints.Length - 1)].position;
            OnSpawned?.Invoke(enemy);
            yield return new WaitForSeconds(spawnDelayTime);
        }
    }
}
