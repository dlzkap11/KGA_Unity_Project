using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerEx : SingletonBehavior<SceneManagerEx>
{

    public void LoadTitleScene()
    {
        
        SceneManager.LoadScene("TitleScene");
    }
    public void LoadGameScene()
    {
        SceneManager.LoadScene("LoadingScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
