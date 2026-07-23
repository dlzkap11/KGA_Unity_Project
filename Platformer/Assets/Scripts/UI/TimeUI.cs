using System.Threading;
using TMPro;
using UnityEngine;

public class TimeUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI bestScore;
    [SerializeField] Goal goal;
    public float timer;

    void Start()
    {
        bestScore.text = $"Best : {PlayerPrefs.GetFloat("Score").ToString("F2")}";
    }

    
    void Update()
    {
        if (goal.isDone)
            return;
        timer += Time.deltaTime;
        timeText.text = timer.ToString("F2");
    }
}
