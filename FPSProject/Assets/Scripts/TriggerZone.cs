using UnityEngine;
using UnityEngine.Events;

public class TriggerZone : MonoBehaviour
{
    public UnityEvent OnTrigger;
    public bool isOnce;

    private void OnTriggerEnter(Collider other)
    {

        if(other.CompareTag("Player"))
        {
            OnTrigger?.Invoke();
            if (isOnce)
                gameObject.SetActive(false);
        }
    }
}
