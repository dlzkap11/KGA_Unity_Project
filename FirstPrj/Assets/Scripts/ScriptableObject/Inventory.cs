using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;

public class Inventory : MonoBehaviour
{

    [SerializeField] private GameObject inventoryPanel;


    // 안정성 그냥 public으로 해도 되긴 하다
    public readonly List<ItemC> inventory = new List<ItemC>();
    public IReadOnlyList<ItemC> InventoryItems => inventory;

    public event Action<ItemC> OnItemAdded;
    public event Action<ItemC> OnItemRemoved;


    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            inventoryPanel.SetActive(!inventoryPanel.activeSelf);
        }
    }

    // 아이템을 인벤토리에 추가
    public void Add(ItemC item)
    {
        int index = inventory.FindIndex(t => t.ItemData.ItemId.Equals(item.ItemData.ItemId));
        if (index != -1)
        {
            inventory[index].Quatity++;
            Debug.Log($"중복아이템 갯수 + 1{inventory[index].Quatity}");
            OnItemAdded?.Invoke(item);
            return;
        }
            
        
        inventory.Add(item);
        Debug.Log($"획득 : {item.ItemData.ItemName}");
        OnItemAdded?.Invoke(item);
    }

    // 아이템을 인벤토리에서 삭제
    public void Remove(ItemC item)
    {
        if (!inventory.Remove(item))
            return;
        
        Debug.Log($"삭제 : {item.ItemData.ItemName}");
        OnItemRemoved?.Invoke(item);
    }
}
