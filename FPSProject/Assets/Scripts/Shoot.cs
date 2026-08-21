using UnityEngine;

public class Shoot : MonoBehaviour
{
    [SerializeField] ObjectPool pool;

    void Start()
    {
        
    }

    void Update()
    {
        pool.Fuck(transform.position, transform.rotation);
        //pool.Get(transform.position, transform.rotation, 3f);
        if (Input.GetKeyDown(KeyCode.X))
        {
            
        }
    }
}
