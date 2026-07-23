using UnityEngine;

// 직접 사용되지 않고, 반드시 MonoBehavior를 상속받도록 강제함.
public abstract class SingletonBehavior<T> : MonoBehaviour where T : MonoBehaviour
{
    // 내부에서 사용하는 인스턴스
    private static T instance;

    // 외부로 접근점 열어둠
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                // 이미 씬에 배치되었는지 찾음.
                instance = FindAnyObjectByType<T>();

                // 씬에 없으면 생성
                if (instance == null)
                {
                    GameObject singleton = new GameObject(typeof(T).Name);
                    instance = singleton.AddComponent<T>();
                }
            }

            return instance;
        }
    }

    // 싱글톤 만들기
    protected virtual void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this as T;
        DontDestroyOnLoad(gameObject);
    }

    // 삭제되었을때 처리
    protected virtual void OnDestroy()
    {
        if (instance == this as T)
        {
            instance = null;
        }
    }
}