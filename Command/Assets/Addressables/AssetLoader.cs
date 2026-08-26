using UnityEngine;
using UnityEngine.AddressableAssets;

public class AssetLoader : MonoBehaviour
{
    [SerializeField] GameObject player;

    // 직접 참조하는 방식 - 굳이?
    [SerializeField] AssetReference assetReference;

    private void Awake()
    {
        // 비동기 - 다른 로딩할 데이터도 동시에 가져올 수 있음(시간단축)
        Addressables.LoadAssetAsync<GameObject>("Player").Completed += x => player = x.Result;

        // 냅다 생성하기
        Addressables.InstantiateAsync("Player", Vector3.zero, Quaternion.identity);
        // 반환
        Addressables.ReleaseInstance(player);

        // 동기 - 비동기가 익숙하지않으면 이렇게 = 권장하는 방식은 아님
        //player = Addressables.LoadAssetAsync<GameObject>("Player").WaitForCompletion();
    }

    void Update()
    {
        
    }
}
