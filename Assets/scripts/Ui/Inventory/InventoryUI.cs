using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public GameObject slotPrefab;
    public Transform parent;

    void OnEnable()
    {
        RefreshUI();
    }

    public void RefreshUI()
    {
        if (Inventory.instance == null) return;

        foreach (Transform child in parent)
            Destroy(child.gameObject);

        foreach (InventoryItem item in Inventory.instance.items)
        {
            if (!item.isEquipped)
            {
                CreateSlot(item);
            }
        }
    }

    void CreateSlot(InventoryItem item)
    {
        GameObject slot = Instantiate(slotPrefab, parent);

        InventorySlotUI slotUI = slot.GetComponent<InventorySlotUI>();

        if (slotUI != null)
        {
            slotUI.Setup(item);
        }
        else
        {
            Debug.LogError("InventorySlotUI manquant sur le prefab slot !");
        }
    }
}