using UnityEngine;
using UnityEngine.UI;

public class TitleUI : MonoBehaviour
{

    [SerializeField] private Button start;
    [SerializeField] private Button quit;

    private void OnEnable()
    {
        start.onClick.AddListener(SceneManagerEx.Instance.LoadGameScene);
        quit.onClick.AddListener(SceneManagerEx.Instance.QuitGame);
    }

    private void OnDisable()
    {
        start.onClick.RemoveAllListeners();
        quit.onClick.RemoveAllListeners();
    }
}
