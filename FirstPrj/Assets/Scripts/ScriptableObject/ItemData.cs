using System;
using UnityEngine;

//아이템 타입 Define에서 관리하기
public enum ItemType
{
    Weapon,
    Armor,
    Potion,
}

[CreateAssetMenu(menuName = "GameData/ItemData")]
public class ItemData : ScriptableObject
{

    //Id
    public int ItemId;
    //아이템 이름
    public string ItemName;
    //아이템 타입
    public ItemType itemType;
    //아이템 아이콘
    public Sprite ItemIcon;
    //아이템 수치 => 무기 - 공격력, 방어구 - 방어력, 포션 - 회복량
    public int ItemValue;
    //아이템 가격
    public int ItemPrice;
    //아이템 설명
    [TextArea] public string ItemDesc;
    
    
}
