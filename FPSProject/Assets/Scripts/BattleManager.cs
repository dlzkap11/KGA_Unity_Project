using System.Collections;
using TMPro;
using UnityEngine;
//[BattleManager 의사코드]

//필드: 스폰 매니저, 남은 적 수, 현재 웨이브(1), 총 웨이브 수(3), 웨이브 휴식 시간(3초)  
//      UI: 체력 텍스트, 남은 적 텍스트, 웨이브 텍스트, CLEAR 패널

//켜질 때:  
//  스폰 매니저의 "적이 소환됐다" 방송을 구독한다

//적이 소환되면(방송 수신):  
//  남은 적 수 + 1, UI 갱신  
//  그 적의 Health가 내는 "죽었다"(OnDied) 방송을 구독한다   // 태어나는 순간이 구독의 타이밍

//적이 죽으면(방송 수신):  
//  남은 적 수 −1, UI 갱신  
//  0이 되면 → "웨이브 전멸" 처리

//웨이브 전멸:  
//  마지막 웨이브였으면 → CLEAR 패널을 켜고 종료             // "다 죽였다"와 "다 끝났다"는 다른 문장  
//  아니면 → 현재 웨이브 +1, 스폰 매니저의 웨이브를 1명 키우고,
//           "휴식 후 다음 웨이브" 코루틴 시작

//휴식 후 다음 웨이브 (코루틴):  
//  웨이브 텍스트 갱신 — 값이 바뀌는 지금 이 순간에만          // 왜 Update가 아닌지는 내일 Profiler에서. 복선  
//  휴식 시간만큼 기다린 뒤 → 스폰 매니저 웨이브 시작

public class BattleManager : MonoBehaviour
{
    [SerializeField] EnemySpawnManager spawnManager;
    [SerializeField] int enemyCount;
    [SerializeField] int currentWave;
    [SerializeField] int totalWave;
    [SerializeField] float restTime;

    [Header("UI")]
    [SerializeField] TMP_Text playerHpText;
    [SerializeField] TMP_Text enemyCountText;
    [SerializeField] TMP_Text currentWaveText;
    [SerializeField] GameObject clearPanel;

    private void Start()
    {
        UpdateEnemyCount();
        UpdateWaveCount();
    }

    private void OnEnable()
    {
        spawnManager.OnSpawned.AddListener(HandleEnemySpawned);
    }

    private void OnDisable()
    {
        spawnManager.OnSpawned.RemoveListener(HandleEnemySpawned);
    }


    public void HandleEnemySpawned(GameObject go)
    {
        enemyCount++;
        UpdateEnemyCount();

        Health health = go.GetComponent<Health>();
        if(health != null)
        {
            health.OnDied += HandleEnemyDied; //죽어서 사라지면 굳이 안빼줘도 된다.
        }
    }

    public void HandleEnemyDied()
    {
        enemyCount--;
        UpdateEnemyCount();

        if (enemyCount == 0)
        {
            ClearWave();
        }
            
    }

    public void UpdateEnemyCount()
    {
        enemyCountText.text = "Enemy : " + enemyCount.ToString();
    }

    public void UpdateWaveCount()
    {
        currentWaveText.text = "Wave :" + currentWave.ToString();
    }

    public void ClearWave()
    {
        if(currentWave != totalWave)
        {
            ++currentWave;
            ++spawnManager.waveAmount;
            UpdateWaveCount();
            StartCoroutine(RestTime());
            
        }
        else
        {
            clearPanel.SetActive(true);
        }
        
    }


    public IEnumerator RestTime()
    {
        yield return new WaitForSeconds(restTime);
        spawnManager.StartWave();
    }
}
