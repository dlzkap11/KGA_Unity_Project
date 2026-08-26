using UnityEngine;
using UnityEngine.AddressableAssets;

public interface IResourceManager
{
    public GameObject Instantiate(string name, Vector3 pos, Quaternion rot);

    public void Destroy(GameObject gameObject);
}

public class ResourceManger : MonoBehaviour, IResourceManager
{
    private static ResourceManger instance;
    public static ResourceManger Instance => instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public void Destroy(GameObject gameObject)
    {
        Addressables.ReleaseInstance(gameObject);
    }

    public GameObject Instantiate(string name, Vector3 pos, Quaternion rot)
    {
        //GameObject prefab = Resources.Load<GameObject>(name);
        //return Instantiate(prefab, pos, rot);

        return Addressables.InstantiateAsync(name, pos, rot).WaitForCompletion();
    }
}

