using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public GameObject slotPrefab;
    public Transform parent;

    void Start()
    {
        RefreshUI();
    }

    public void RefreshUI()
    {
        if (Inventory.instance == null)
        {
            Debug.LogError("Inventory.instance est NULL !");
            return;
        }

        if (parent == null)
        {
            Debug.LogError("Parent UI NON assigné !");
            return;
        }

        foreach (Transform child in parent)
        {
            Destroy(child.gameObject);
        }

        foreach (Sprite item in Inventory.instance.items)
        {
            GameObject slot = Instantiate(slotPrefab, parent);

            Image img = slot.GetComponent<Image>();
            if (img != null)
            {
                img.sprite = item;
            }
        }
    }
}