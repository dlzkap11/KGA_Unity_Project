using UnityEngine;
using DG.Tweening;

public class DotweenTest : MonoBehaviour
{
    private Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            rb.DOMove(new Vector3(10, 10, 10), 5f);
            //transform.DOMove(new Vector3(10,10,10), 1f).OnComplete(() => Debug.Log("이동완료"));
        }

        if (Input.GetKey(KeyCode.RightControl))
        {
            transform.DOScale(new Vector3(10, 10, 10), 1f).SetEase(Ease.InQuad);
        }
        
        
    }
}
