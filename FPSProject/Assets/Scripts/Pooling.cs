using System.Collections;
using UnityEngine;

public class Pooling : MonoBehaviour
{
    

    void Start()
    {
        
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * 3f * Time.deltaTime);
    }

}
