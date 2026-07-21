using Unity.Android.Gradle.Manifest;
using UnityEditor;
using UnityEngine;

public class ItemC
{
    public ItemData ItemData { get; }
    public int Quatity { get; set; } //독립적

    protected Inventory inventory;

    public ItemC(ItemData itemData, int quatity = 1)
    {
        ItemData = itemData;
        Quatity = quatity;
    }

    public virtual void Use(GameObject user)
    {
        Debug.Log($"{ItemData.ItemName}을 사용했다.");
        inventory = user.GetComponent<Inventory>();
    }

}


public class PotionItem : ItemC
{
    public PotionItem(ItemData itemData, int quatity = 1) : base(itemData, quatity) { }


    public override void Use(GameObject user)
    {
        base.Use(user);
        Debug.Log($"{ItemData.ItemName} : 체력을 {ItemData.ItemValue}만큼 회복합니다.");
        ItemData.ItemValue += 50; //평생 늘어남
        Quatity--;
        if(Quatity <= 0 )
            inventory.Remove(this);
    }
}

public class WeaphonItem : ItemC
{
    public WeaphonItem(ItemData itemData, int quatity = 1) : base(itemData, quatity) { }


    public override void Use(GameObject user)
    {
        base.Use(user);
        Debug.Log($"{ItemData.ItemName}을 장착합니다. 공격력 : {ItemData.ItemValue}.");
        Quatity--;
        if (Quatity <= 0)
            inventory.Remove(this);
    }
}

public class ArmorItem : ItemC
{
    public ArmorItem(ItemData itemData, int quatity = 1) : base(itemData, quatity) { }


    public override void Use(GameObject user)
    {
        base.Use(user);
        Debug.Log($"{ItemData.ItemName}을 장착합니다. 방어력 : {ItemData.ItemValue}.");
        Quatity--;
        if (Quatity <= 0)
            inventory.Remove(this);
    }
}

