using UnityEngine;
using UnityEngine.UI;

public class TitleUI : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private Button quitButton;


    private void OnEnable()
    {
        startButton.onClick.AddListener(SceneManagerEx.Instance.LoadGameScene);
        quitButton.onClick.AddListener(SceneManagerEx.Instance.QuitGame);
    }

    private void OnDisable()
    {
        startButton.onClick.RemoveAllListeners();
        quitButton.onClick.RemoveAllListeners();
    }

    void Update()
    {
        
    }
}
