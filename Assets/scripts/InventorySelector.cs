using UnityEngine;

public class InventorySelector : MonoBehaviour
{
    public static InventorySelector instance;

    public InventoryItem selectedItem;

    void Awake()
    {
        instance = this;
        selectedItem = null;

        Debug.Log("InventorySelector OK");
    }

    public void SelectItem(InventoryItem item)
    {
        selectedItem = item;
        Debug.Log("Sélectionné : " + item.itemName);
    }
}