using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;
//[AddressableSpawner 의사코드]

//필드: prefabAddress(문자열), spawnPoint(Transform)
//      spawnedInstance(캐싱), spawnHandle(캐싱, 있을 수도 없을 수도)

//SpawnAsync(비동기 함수):  
//  이미 유효한 spawnHandle이 있으면 → 경고 로그를 남기고 종료      (가드 절 — 중복 로딩 방지)  
//  위치 ← spawnPoint가 있으면 그 위치, 없으면 내 위치  
//  handle ← 주소로 InstantiateAsync 요청 (위치 포함)  
//  spawnHandle ← handle  
//  handle의 완료를 기다린다 (await)  
//  handle 상태가 성공이면 → spawnedInstance ← handle 결과  
//  아니면 → 실패를 로그로 남긴다 (과제에서 UI로도 알린다)

//ReleaseSpawned:  
//  spawnedInstance가 있으면 →  
//      Addressables로 인스턴스를 반납한다 (Destroy가 아니다)  
//      spawnedInstance, spawnHandle을 비운다

public class AddressableSpawner : MonoBehaviour
{
    [SerializeField] string prefabAddress;
    [SerializeField] Transform spawnPoint;
    [SerializeField] GameObject spawnedInstance;
    [SerializeField] AsyncOperationHandle<GameObject>? spawnHandle;

    public async void SpawnAsync()
    {
        if (spawnHandle != null)
        {
            Debug.LogWarning("이미 스폰된 핸들 존재");
            return;
        }

        if (spawnPoint == null)
            spawnPoint = transform;

        spawnHandle = Addressables.InstantiateAsync(prefabAddress, spawnPoint.position, Quaternion.identity);
        await spawnHandle?.Task;
        if (spawnHandle?.Status == AsyncOperationStatus.Succeeded)
        {
            spawnedInstance = spawnHandle?.Result;
        }   
        else if(spawnHandle?.Status == AsyncOperationStatus.Failed)
        {
            Debug.Log("생성 실패!");
        }

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnAsync();
        }

        if(Input.GetKeyDown(KeyCode.K))
        {
            ReleaseSpawned();
        }
    }

    public void ReleaseSpawned()
    {
        if(spawnHandle != null)
        {
            Addressables.ReleaseInstance(spawnedInstance);
            spawnedInstance = null;
            spawnHandle = null;
        }
        
    }
}
