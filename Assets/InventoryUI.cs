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
        {
            Destroy(child.gameObject);
        }

        // 💧 afficher les gouttes
        for (int i = 0; i < Inventory.instance.waterDropCount; i++)
        {
            CreateSlot(Inventory.instance.waterDropIcon);
        }

        // 🧪 afficher H2O restants
        for (int i = 0; i < Inventory.instance.h2oCount; i++)
        {
            CreateSlot(Inventory.instance.h2oIcon);
        }
    }

    void CreateSlot(Sprite icon)
    {
        GameObject slot = Instantiate(slotPrefab, parent);
        slot.GetComponent<Image>().sprite = icon;
    }
}