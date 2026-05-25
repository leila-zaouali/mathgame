using UnityEngine;

public class ItemInHand : MonoBehaviour
{
    public Transform handPoint;

    private GameObject currentObject;
    private InventoryItem currentItem;

    void Update()
    {
        if (GameInput.instance.UsePressed())
        {
            if (currentObject == null)
            {
                // 🔥 EQUIPER
                InventoryItem item = InventorySelector.instance.selectedItem;

                if (item == null || item.prefab == null) return;

                item.isEquipped = true;

                currentItem = item;

                currentObject = Instantiate(item.prefab, handPoint.position, handPoint.rotation);
                currentObject.transform.SetParent(handPoint);

                InventoryUIRefresh();
            }
            else
            {
                // 🔥 REPOSER
                currentItem.isEquipped = false;

                Destroy(currentObject);
                currentObject = null;
                currentItem = null;

                InventoryUIRefresh();
            }
        }
    }

    void InventoryUIRefresh()
    {
        FindObjectOfType<InventoryUI>().RefreshUI();
    }
}