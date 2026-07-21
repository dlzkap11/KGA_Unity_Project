using UnityEngine;

public class InventoryLogger : MonoBehaviour
{
    [SerializeField]
    private Inventory inventory;
    

    private void OnEnable()
    {
        inventory.OnItemAdded += ItemAddedHandler;
        inventory.OnItemRemoved += ItemRemovedHandler;
    }


    private void OnDisable()
    {
        inventory.OnItemAdded -= ItemAddedHandler;
        inventory.OnItemRemoved -= ItemRemovedHandler;
    }

    private void ItemAddedHandler(ItemC item)
    {
        Debug.Log($"알림 수신 : {item.ItemData.ItemName} 추가");
    }

    private void ItemRemovedHandler(ItemC item)
    {
        Debug.Log($"알림 수신 : {item.ItemData.ItemName} 삭제");
    }
}
