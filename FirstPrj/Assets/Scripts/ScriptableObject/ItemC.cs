using Unity.Android.Gradle.Manifest;
using UnityEngine;

public class ItemC
{
    public ItemData ItemData { get; }
    public int Quatity { get; set; }

    public ItemC(ItemData itemData, int quatity)
    {
        ItemData = itemData;
        Quatity = quatity;
    }

    public virtual void Use(GameObject user)
    {
        Debug.Log($"{ItemData.ItemName}을 사용했다.");
    }


}


public class PotionItem : ItemC
{
    public PotionItem(ItemData itemData, int quatity) : base(itemData, quatity) { }


    public override void Use(GameObject user)
    {
        base.Use(user);
        Debug.Log($"{ItemData.ItemName} : 체력을 {ItemData.ItemValue}만큼 회복합니다.");
    }
}

public class WeaphonItem : ItemC
{
    public WeaphonItem(ItemData itemData, int quatity) : base(itemData, quatity) { }


    public override void Use(GameObject user)
    {
        Debug.Log($"{ItemData.ItemName}을 장착합니다. 공격력 : {ItemData.ItemValue}.");
    }
}

public class ArmorItem : ItemC
{
    public ArmorItem(ItemData itemData, int quatity) : base(itemData, quatity) { }


    public override void Use(GameObject user)
    {
        Debug.Log($"{ItemData.ItemName}을 장착합니다. 방어력 : {ItemData.ItemValue}.");
    }
}

