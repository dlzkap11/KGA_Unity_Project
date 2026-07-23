using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UITest : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Slider hpSlider;
    [SerializeField] private Button restartButton;
    [SerializeField] private Image image;
    [SerializeField] private Toggle toggle;
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private TMP_Dropdown dropDown;
    [SerializeField] private Button quitButton;

    private int score;
    private int hp;

    void Start()
    {
        
        hp = 100;
        SetHealth(100);
    }

    private void OnEnable()
    {
        hpSlider.onValueChanged.AddListener(SetHealth);
        restartButton.onClick.AddListener(ResetUI);
        quitButton.onClick.AddListener(SceneManagerEx.Instance.LoadTitleScene);
    }

    private void OnDisable()
    {
        hpSlider.onValueChanged.RemoveAllListeners();
        restartButton.onClick.RemoveAllListeners();
        quitButton.onClick.RemoveAllListeners();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            AddScore(10);
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            hp -= 10;
            SetHealth(100);
        }
    }

    public void AddScore(int amount)
    { 
        score += amount;
        scoreText.text = $"Score : {score}";
    }

    public void SetHealth(float max)
    {
        hpSlider.value = (float)hp / max;

    }

    public void ResetUI()
    {
        score = 0;
        AddScore(0);
        hp = 100;
        SetHealth(100);
    }
}
