using NUnit.Framework.Interfaces;
using Unity.VisualScripting;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private ItemData itemData;
    //Id
    [SerializeField] private int itemId;
    //아이템 이름
    [SerializeField] private string itemName;
    //아이템 타입
    [SerializeField] private ItemType itemType;
    //아이템 아이콘
    [SerializeField] private Sprite itemIcon;
    //아이템 수치 => 무기 - 공격력, 방어구 - 방어력, 포션 - 회복량
    [SerializeField] private int itemValue;
    //아이템 가격
    [SerializeField] private int itemPrice;
    //아이템 설명
    [SerializeField] private string itemDesc;


    // 참조타입 값들이 변경되어도 변수로 따로 가져간다
    private void Awake()
    {
        itemId = itemData.ItemId;
        itemName = itemData.ItemName;
        itemType = itemData.itemType;
        itemIcon = itemData.ItemIcon;
        itemValue = itemData.ItemValue;
        itemPrice = itemData.ItemPrice;
        itemDesc = itemData.ItemDesc;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        Debug.Log($"아이템 습득:{itemData.ItemName}, {itemData.ItemValue}");
        itemData.ItemValue += 50; //전체 50 증가
        Destroy(gameObject);
    }
}
