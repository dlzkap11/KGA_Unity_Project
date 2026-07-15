using UnityEngine;

public class Collision : MonoBehaviour
{
    private void OnCollisionEnter(UnityEngine.Collision collision)
    {
        Debug.Log("Collision 입장");
    }

    private void OnCollisionExit(UnityEngine.Collision collision)
    {
        Debug.Log("Collisiong 퇴장");
    }

    private void OnCollisionStay(UnityEngine.Collision collision)
    {
        Debug.Log("Collisiong 유지");
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"{other.name}Trigger 입장");
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Trigger 퇴장");
    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log("Trigger 유지");
    }
}
