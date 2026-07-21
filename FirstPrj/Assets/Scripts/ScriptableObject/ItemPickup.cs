using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [SerializeField] private ItemData itemData;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        Debug.Log($"Pick up: {itemData.ItemName} ({itemData.itemType})");
        ItemC pickUpItem = CreateItem();

        pickUpItem.Use(other.gameObject);
        Destroy(gameObject);
    }

    private ItemC CreateItem()
    {
        if (itemData.itemType == ItemType.Weaphon)
            return new WeaphonItem(itemData, 1);
        if (itemData.itemType == ItemType.Potion)
            return new PotionItem(itemData, 1);
        if (itemData.itemType == ItemType.Armor)
            return new ArmorItem(itemData, 1);


        return null;
    }
}