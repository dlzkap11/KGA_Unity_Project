using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System.Text;


public class ItemSlot : MonoBehaviour,IPointerClickHandler
{
    [SerializeField] private Image iconImage;
    private ItemC bindItem;
    private GameObject player;
    [SerializeField]
    TextMeshProUGUI textMeshProUGUI;

    public void Bind(ItemC item, GameObject player)
    {
        bindItem = item;
        this.player = player;
        iconImage.sprite = bindItem.ItemData.ItemIcon;
        iconImage.enabled = true;
        Debug.Log($"{item.ItemData.ItemName} 바운디");
        textMeshProUGUI = gameObject.GetComponentInChildren<TextMeshProUGUI>();
        textMeshProUGUI.text = item.Quatity.ToString();
    }

    //왜 안댐
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.clickCount == 2)
        {
            
            bindItem.Use(player); // Quatity--;
            Debug.Log($"{bindItem.Quatity}개 남았습니다.");
            if (bindItem.Quatity <= 0)
            {
                Destroy(gameObject);
            }
            textMeshProUGUI.text = bindItem.Quatity.ToString();
            
        }
    }
}
