using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventorySlot
{
    public ItemData item;
    public int quantity;

    public InventorySlot(ItemData item, int quantity)
    {
        this.item = item;
        this.quantity = quantity;
    }
}

[System.Serializable]
public class Inventory
{
    public List<InventorySlot> slots = new List<InventorySlot>();

    public void AddItem(ItemData item, int quantity)
    {
        InventorySlot slot = slots.Find(s => s.item == item);
        if (slot != null)
        {
            slot.quantity += quantity;
        }
        else
        {
            slots.Add(new InventorySlot(item, quantity));
        }
    }

    public void RemoveItem(ItemData item, int quantity)
    {
        InventorySlot slot = slots.Find(s => s.item == item);
        if (slot != null)
        {
            slot.quantity -= quantity;
            if (slot.quantity <= 0)
                slots.Remove(slot);
        }
    }
}

