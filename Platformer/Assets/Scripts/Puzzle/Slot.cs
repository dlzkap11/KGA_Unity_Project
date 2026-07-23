using UnityEngine;
using static Unity.Burst.Intrinsics.X86;

public class Slot : MonoBehaviour
{
    private bool isOk;
    public static int BoxCnt {  get; private set; }

    private void Awake()
    {
        BoxCnt = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Cube"))
        {
            isOk = true;
            BoxCnt++;
            GetComponent<Renderer>().material.color = Color.green;
            Debug.Log(BoxCnt);
        }
            
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Cube"))
        {
            isOk = false;
            BoxCnt--;
            Debug.Log(BoxCnt);
            GetComponent<Renderer>().material.color = Color.red;
        }
            
    }
}
