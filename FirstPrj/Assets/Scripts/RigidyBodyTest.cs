using UnityEngine;

public class RigidyBodyTest : MonoBehaviour
{

    public Rigidbody rb;

    void Start()
    {
        gameObject.AddComponent<Rigidbody>();
        rb = GetComponent<Rigidbody>();
    }

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.useGravity = !rb.useGravity;
        }
    }
}
