using UnityEngine;

public class ItemInHand : MonoBehaviour
{
    public Transform handPoint;
    public Transform headPoint;

    private GameObject currentObject;
    private InventoryItem currentItem;

    void Update()
    {
        if (GameInput.instance == null) return; // ✅ AJOUTE

        if (GameInput.instance.UsePressed())
        {
            // 🔥 PRENDRE OBJET
            if (currentObject == null)
            {
                InventoryItem item = InventorySelector.instance.selectedItem;

                if (item == null) return;
                if (item.prefab == null) return;

                currentItem = item;

                // 🔦 TORCHE → TÊTE
                if (item.itemName == "torche")
                {
                    currentObject = Instantiate(
                        item.prefab,
                        headPoint.position,
                        headPoint.rotation
                    );

                    currentObject.transform.SetParent(headPoint);
                }
                // ✋ AUTRES OBJETS → MAIN
                else
                {
                    currentObject = Instantiate(
                        item.prefab,
                        handPoint.position,
                        handPoint.rotation
                    );

                    currentObject.transform.SetParent(handPoint);
                }

                currentObject.transform.localPosition = Vector3.zero;
                currentObject.transform.localRotation = Quaternion.identity;

                item.isEquipped = true;

                Object.FindAnyObjectByType< InventoryUI>().RefreshUI();
            }
            // 🔥 REMETTRE OBJET
            else
            {
                currentItem.isEquipped = false;

                Destroy(currentObject);

                currentObject = null;
                currentItem = null;

                Object.FindAnyObjectByType<InventoryUI>().RefreshUI();
            }
        }
    }
}