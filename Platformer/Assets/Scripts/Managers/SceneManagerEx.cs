using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerEx : SingletonBehavior<SceneManagerEx>
{

    public void LoadTitleScene()
    {
        
        SceneManager.LoadScene("Title");
    }
    public void LoadGameScene()
    {
        SceneManager.LoadScene("Game");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
