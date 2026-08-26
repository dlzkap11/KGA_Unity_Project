using JetBrains.Annotations;
using System;
using System.Collections;
using UnityEngine;
/*
[AmmoSystem 의사코드]

필드: 탄창크기, 재장전시간  
상태: 현재탄약, 재장전중(bool)                    // 밖에서는 읽기만 가능하게  
방송: "탄약이 바뀌었다"(현재 탄약, 탄창 크기)  
      "재장전 상태가 바뀌었다"(bool)              // 51차시 이벤트 발행·구독 그대로

Awake: 현재탄약 ← 탄창크기

한 발 쓰기(계약): 쏠 수 있으면 한 발 쓰고 참, 아니면 거짓  
  쏠 수 없는 경우 = 재장전 중이거나, 탄창이 비었을 때  
  한 발 썼으면 → "탄약이 바뀌었다" 방송  
  // 사격 코드는 탄약 규칙을 몰라도 된다 — 물어보고 한 줄로 끝낸다

재장전 시작:  
  문 앞에 문지기를 세운다 — 몇 명을, 무엇을 막게 세울지는 아래 목표가 정한다  
  통과하면 → 재장전 절차(코루틴) 시작

재장전 절차(50차시 그 절차서):  
  빼고    → 재장전중 ← 참, 상태 방송  
  기다리고 → 재장전시간만큼 대기  
  채우고  → 현재탄약 ← 탄창크기, 재장전중 ← 거짓, 방송 2종  
  // 하나라도 건너뛰면 안 되는 순서다
*/

public class AmmoSystem : MonoBehaviour
{

    [SerializeField] private float reloadTime;
    [SerializeField] public int maxMagazine;
    public int CurAmmo {  get; private set; }
    public bool isReload {  get; private set; }

    public event Action<int, int> OnChangeAmmo;
    public event Action OnReload;

    private void Awake()
    {
        CurAmmo = maxMagazine;
    }


    public void Fire()
    {
        if (isReload || CurAmmo <= 0)
            return;

        CurAmmo--;
        OnChangeAmmo?.Invoke(CurAmmo, maxMagazine);

    }

    public void Reloding()
    {
        if (isReload)
            return;
        if (CurAmmo == maxMagazine)
            return;


        isReload = true;
        StartCoroutine(Reloader());
    }

    IEnumerator Reloader()
    {
        yield return new WaitForSeconds(reloadTime);
        CurAmmo = maxMagazine;
        OnReload?.Invoke();
        isReload = false;
    }

}
