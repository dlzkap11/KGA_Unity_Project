using UnityEngine;

public class Goal : MonoBehaviour
{
    
    [SerializeField] TimeUI timeUI;
    public bool isDone;
    public float MinTime;

    private void Awake()
    {
        if (!PlayerPrefs.HasKey("Score"))
        {
            MinTime = float.MaxValue;
        }
        else
            MinTime = PlayerPrefs.GetFloat("Score");
    }

    void Update()
    {
        if(Slot.BoxCnt == 2)
        {
            GetComponent<Renderer>().material.color = Color.green;
        }
        else
        {
            GetComponent<Renderer>().material.color = Color.red;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && Slot.BoxCnt == 2)
        {
            isDone = true;
            if(MinTime > timeUI.timer)
            {
                PlayerPrefs.SetFloat("Score", timeUI.timer);
                PlayerPrefs.Save();
            }
            Debug.Log("승리!");
        }
            

    }
}
