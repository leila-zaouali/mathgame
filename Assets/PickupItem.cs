using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public ItemData data;

    public void PickUp()
    {
        Inventory.instance.AddItem(data.itemName, data.icon);
        Destroy(gameObject);
    }
}