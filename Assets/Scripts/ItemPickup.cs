using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public ItemData itemData; // Assign the item in the Inspector

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player collided
        PlayerInventory playerInventory = other.GetComponent<PlayerInventory>();
        if (playerInventory != null)
        {
            // Give the item to the player
            playerInventory.PickupItem(itemData);

            // Destroy the world item
            Destroy(gameObject);
        }
    }
}
