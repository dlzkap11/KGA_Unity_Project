using UnityEngine;

public class Collision : MonoBehaviour
{
    private void OnCollisionEnter(UnityEngine.Collision collision)
    {
        Debug.Log("collision");
    }
}
