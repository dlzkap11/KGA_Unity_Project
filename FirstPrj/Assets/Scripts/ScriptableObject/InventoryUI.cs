using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEditor.UIElements;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private Inventory inventory;
    [SerializeField]
    private Transform gridParent;
    [SerializeField]
    private ItemSlot slotPrefab;
    [SerializeField]
    private GameObject player;

    private const int SLOT_MAX_SIZE = 21;
    private List<ItemSlot> slotPool = new List<ItemSlot>();
    private int activeSlotCount = 0;


    private void Awake()
    {
        for (int i = 0; i < SLOT_MAX_SIZE; i++)
        {
            ItemSlot slot = Instantiate(slotPrefab, gridParent);
            slot.gameObject.SetActive(false);
            slotPool.Add(slot);
        }
    }

    private void OnEnable()
    {
        inventory.OnItemAdded += InventoryChangeHandler;
        inventory.OnItemRemoved += InventoryChangeHandler;
    }

    private void OnDisable()
    {
        inventory.OnItemAdded -= InventoryChangeHandler;
        inventory.OnItemRemoved -= InventoryChangeHandler;
    }

    private void InventoryChangeHandler(ItemC item)
    {
        RefreshUI(item);
    }

    
    // 그럼 아싸리 인벤토리 전체는 존재하고
    // 아이템을 누르면 드래그드롭이 가능하고 해당 슬롯으로 이동시켜서 놓으면 거기로 이동되는 시스템
    // 이러면 인벤토리 정렬도 가능 

    private void RemoveSlot(ItemC item)
    {
        int index = inventory.inventory.FindIndex(t => t.ItemData.ItemId.Equals(item.ItemData.ItemId));
        if (index == -1)
            return;

        //패널 안쪽 슬롯에 대한 스크립트

    }


    private void AddSlot(ItemC item)
    {
        

        ItemSlot slot = Instantiate(slotPrefab, gridParent);

        slot.Bind(item, player);
        slot.name = item.ItemData.name;
    }

    private void RefreshUI(ItemC item)
    {

        //ItemSlot slot = Instantiate(slotPrefab, gridParent);
        slotPool[activeSlotCount].gameObject.SetActive(true);
        activeSlotCount++;


        slotPool[activeSlotCount].Bind(item, player);
        slotPool[activeSlotCount].name = item.ItemData.name;
        /*
        foreach (Transform child in gridParent)
        {
            Destroy(child.gameObject);
        }
        */
        /*
        foreach (ItemC item in inventory.InventoryItems)
        {
            //ItemSlot slot = Instantiate(slotPrefab, gridParent);
            slotPool[activeSlotCount].gameObject.SetActive(true);
            activeSlotCount++;


            slotPool[activeSlotCount].Bind(item, player);
            slotPool[activeSlotCount].name = item.ItemData.name;
        }
        */
    }
}
