using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] GameObject prefab;
    [SerializeField] int objAmout;

    private Queue<GameObject> queue = new Queue<GameObject>();

    private void Awake()
    {
        for (int i = 0; i < objAmout; i++)
        {
            GameObject go = Instantiate(prefab, transform);
            queue.Enqueue(go);
            go.SetActive(false);
            
        }
    }

    public GameObject Fuck(Vector3 pos, Quaternion rot)
    {
        GameObject target;
        target = Instantiate(prefab, transform);
        target.transform.position = pos;
        target.transform.rotation = rot;
        StartCoroutine(ReleaseRoutine2(target, 3f));
        return target;
    }

    public GameObject Get(Vector3 pos, Quaternion rot)
    {
        GameObject target;
        if (queue.Count == 0)
        {
            target = Instantiate(prefab, transform);
        }
        else
        {
            target = queue.Dequeue();

        }
        target.transform.position = pos;
        target.transform.rotation = rot;
        target.SetActive(true);

        return target;
    }

    public GameObject Get(Vector3 pos, Quaternion rot, float duration)
    {
        GameObject target;
        if (queue.Count == 0)
        {
            target = Instantiate(prefab, transform);
        }
        else
        {
            target = queue.Dequeue();

        }
        target.transform.position = pos;
        target.transform.rotation = rot;
        target.SetActive(true);


        StartCoroutine(ReleaseRoutine(target, duration));
        return target;
    }

    public IEnumerator ReleaseRoutine(GameObject gameObject, float du)
    {

        yield return new WaitForSeconds(du);
        Release(gameObject);
    }

    public IEnumerator ReleaseRoutine2(GameObject gameObject, float du)
    {

        yield return new WaitForSeconds(du);
        Destroy(gameObject);
    }

    public void Release(GameObject gameObject)
    {
        gameObject.SetActive(false);
        queue.Enqueue(gameObject);
    }
}
