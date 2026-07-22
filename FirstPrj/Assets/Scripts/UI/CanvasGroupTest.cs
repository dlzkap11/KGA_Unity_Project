using Mono.Cecil;
using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine;
using DG.Tweening;
using DG.Tweening.Plugins.Options;


public class CanvasGroupTest : MonoBehaviour
{
    [SerializeField] private CanvasGroup group;
    [SerializeField] private float duration;

    private Coroutine currentCoroutine;
    private bool isFade;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            
            group.DOKill();
            isFade = !isFade;
            if (isFade)
            {
                FadeOut();
            }
            else
            {
                FadeIn();
            }
        }
    }

    private void FadeOut()
    {
        
        group.interactable = false;
        group.blocksRaycasts = false;
        group.DOFade(0f, duration);
        
        /*
        if(currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }
        currentCoroutine = StartCoroutine(FadeOutRoutine());
        */
        
    }

    private void FadeIn()
    {

        
        group.DOFade(1f, duration).OnComplete(() => {

            Debug.Log("페이드인 완");
            group.interactable = true;
            group.blocksRaycasts = true;
        });
        
        /*
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }
        currentCoroutine = StartCoroutine(FadeInRoutine());
        */
        
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
