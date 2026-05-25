using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    public List<InventoryItem> items = new List<InventoryItem>();

    public int h2oCount = 0;
    public int totalH2OCollected = 0;

    public int waterDropCount = 0;

    public InventoryItem waterDropItem;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void AddItem(string itemName, Sprite icon, GameObject prefab)
    {
        // 📦 ajouter item dans inventaire
        InventoryItem newItem = new InventoryItem();
        newItem.itemName = itemName;
        newItem.icon = icon;
        newItem.prefab = prefab;

        items.Add(newItem);

        // 🧪 CAS H2O
        if (itemName == "H2O")
        {
            h2oCount++;
            totalH2OCollected++;

            Debug.Log("H2O +1 => " + h2oCount);

            // 💧 TRANSFORMATION
            if (h2oCount >= 3)
            {
                h2oCount -= 3;

                int removed = 0;

                // 🔥 supprimer 3 H2O VISUELS de la liste
                for (int i = items.Count - 1; i >= 0; i--)
                {
                    if (items[i].itemName == "H2O")
                    {
                        items.RemoveAt(i);
                        removed++;

                        if (removed == 3)
                            break;
                    }
                }

                // 💧 ajouter une goutte
                items.Add(waterDropItem);

                waterDropCount++;

                Debug.Log("💧 3 H2O → 1 goutte créée");
            }
        }
    }
}