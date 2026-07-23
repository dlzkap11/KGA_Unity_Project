using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingProgress : MonoBehaviour
{
    [SerializeField] private Slider loadingSlider;


    void Start()
    {
        StartCoroutine(LoadAsync("RigidScene"));
    }

    
    private IEnumerator LoadAsync(string sceneName)
    {
        // operation == 현재 비동기로 씬을 불러오는 과정을 캐싱
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        operation.allowSceneActivation = false;

        while (operation.progress < 0.9f)
        {
            loadingSlider.value = operation.progress;
            yield return null;
        }
        loadingSlider.value = 1f;

        operation.allowSceneActivation = true;

    }

    void Update()
    {
        
    }
}
