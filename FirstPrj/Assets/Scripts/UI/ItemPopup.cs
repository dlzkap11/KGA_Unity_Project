using DG.Tweening;
using System.Net.NetworkInformation;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemPopup : MonoBehaviour
{
    /*
DOTween.Sequence()로 "아래에서 위로 슬라이드하며 등장(Append, Ease.OutBack) + 동시에 페이드인(Join)" → "1초 대기(AppendInterval)" 
    → "위로 슬라이드하며 퇴장 + 페이드아웃(Append+Join)" 순서의 연출을 만든다.
팝업 안의 아이콘 이미지에는 등장 시 DOPunchScale로 살짝 튕기는 효과를 추가한다.
ShowPopup(string itemName) 함수를 만들어, 호출될 때마다 이전 Sequence를 Kill()하고 새로 시작하도록 한다(연속 획득 시 겹침 방지).
테스트용으로 스페이스바를 누르면 ShowPopup("포션")이 호출되게 연결한다.
     */

    [SerializeField] private CanvasGroup group;
    [SerializeField] private float duration;
    [SerializeField] private RectTransform rect;
    [SerializeField] private TextMeshProUGUI text;
    private Image image;
    private Sequence sequence;

    private bool isFade;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            sequence.Kill();
            
            sequence = DOTween.Sequence().Append(rect.DOAnchorPosY(-10f, duration).SetEase(Ease.OutBack))
                .Join(group.DOFade(1f, duration))
                .AppendInterval(1f)
                .Append(rect.DOAnchorPosY(0f, duration).SetEase(Ease.OutBack))
                .Join(group.DOFade(0f, duration));

            //sequence.Play();
            
        }

        //sequence.SetAutoKill(true);

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
    }

    private void FadeIn()
    {
        group.DOFade(1f, duration).OnComplete(() => {

            Debug.Log("페이드인 완");
            group.interactable = true;
            group.blocksRaycasts = true;
        });
    }
}
