using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [SerializeField] private ItemData itemData;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        ItemC pickUpItem = CreateItem();

        other.GetComponent<Inventory>().Add(pickUpItem);

        //pickUpItem.Use(other.gameObject);
        Destroy(gameObject);
    }

    private ItemC CreateItem()
    {
        if (itemData.itemType == ItemType.Weapon)
            return new WeaphonItem(itemData);
        if (itemData.itemType == ItemType.Potion)
            return new PotionItem(itemData);
        if (itemData.itemType == ItemType.Armor)
            return new ArmorItem(itemData, 2);


        return null;
    }
}