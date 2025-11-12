using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    // This is your player's actual inventory.
    public Inventory inventory = new Inventory();

    // Example: pick up an item
    public void PickupItem(ItemData item)
    {
        inventory.AddItem(item, 1);
        Debug.Log($"Picked up {item.itemName}");
    }
}

