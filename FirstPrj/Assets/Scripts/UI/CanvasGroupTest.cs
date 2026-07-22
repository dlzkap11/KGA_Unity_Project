using Mono.Cecil;
using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine;

public class CanvasGroupTest : MonoBehaviour
{
    [SerializeField] private CanvasGroup group;
    [SerializeField] private float duration;

    private Coroutine currentCoroutine;
    
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.K))
        {
            FadeOut();
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            FadeIn();
        }
    }

    private void FadeOut()
    {
        if(currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }
        currentCoroutine = StartCoroutine(FadeOutRoutine());
    }

    private void FadeIn()
    {
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }
        currentCoroutine = StartCoroutine(FadeInRoutine());
    }

    private IEnumerator FadeOutRoutine()
    {
        group.interactable = false;
        group.blocksRaycasts = false;

        float time = 0;
        float current = group.alpha;
        while(time < duration)
        {
            time += Time.deltaTime;
            //group.alpha = current - (time / duration);
            group.alpha = Mathf.Lerp(current, 0f, time / duration);
            yield return null;
        }
        group.alpha = 0f;

        currentCoroutine = null;
    }

    private IEnumerator FadeInRoutine()
    {
        float time = 0;
        float current = group.alpha;
        
        while (time < duration)
        {
            time += Time.deltaTime;
            group.alpha = Mathf.Lerp(current, 1f, time / duration);
            yield return null;
        }
        group.alpha = 1f;

        group.interactable = true;
        group.blocksRaycasts = true;

        currentCoroutine = null;
    }
}
