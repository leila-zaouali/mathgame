using System.Collections;
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
    public void InitInventory()
    {
        StartCoroutine(LoadInventoryCoroutine());
    }
    IEnumerator Start()
    {
        yield return new WaitForSeconds(1f);

        // ✅ Attendre que GetProgress soit terminé
        while (!APIManager.progressLoaded) yield return null;

        string scene = APIManager.nextScene; // ← maintenant nextScene est correct
        UnityEngine.SceneManagement.SceneManager.LoadScene(scene);
    }




    public void AddItem(string itemName, Sprite icon, GameObject prefab)
    {
        InventoryItem newItem = new InventoryItem();
        newItem.itemName = itemName;
        newItem.icon = icon;
        newItem.prefab = prefab;

        items.Add(newItem);

        if (itemName == "H2O")
        {
            h2oCount++;
            totalH2OCollected++;

            Debug.Log("💧 H2O +1 => " + h2oCount);

            // ❌ SUPPRIMÉ : ScoreManager.AddScore(10)

            if (h2oCount >= 3)
            {
                h2oCount -= 3;

                int removed = 0;

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
                if (waterDropCount > 0)
                {
                    Debug.Log("💧 Déjà une goutte, skip");
                    return;
                }
                items.Add(waterDropItem);
                waterDropCount++;

                Debug.Log("💧 3 H2O → 1 goutte créée");

                // ❌ SUPPRIMÉ : ScoreManager.AddScore(50)
            }
        }
        SaveInventory();
    }
    public void SetWaterDropFromSave()
    {
        if (waterDropCount > 0) return;

        items.Add(waterDropItem);
        waterDropCount = 1;

        Debug.Log("💧 Goutte restaurée depuis Firebase");
    }
    public void SaveInventory()
    {
        string data = "";
        foreach (var item in items)
        {
            data += item.itemName + ";";
        }

        //// Backup local
        //PlayerPrefs.SetString("inventory_data", data);
        //PlayerPrefs.Save();

        // ✅ Sauvegarde sur le serveur
        if (APIManager.instance != null)
            APIManager.instance.SaveInventory(data);

        Debug.Log("💾 Inventory sauvegardé (local + serveur)");
    }
    public void LoadInventory()
    {
        StartCoroutine(LoadInventoryCoroutine());
    }

    IEnumerator LoadInventoryCoroutine()
    {
        while (ItemDatabase.instance == null)
        {
            Debug.Log("⏳ Waiting ItemDatabase...");
            yield return null;
        }

        items.Clear();

        string data = PlayerPrefs.GetString("inventory_data", "");

        Debug.Log("📥 LOAD RAW = " + data);

        if (string.IsNullOrEmpty(data))
            yield break;

        string[] names = data.Split(';');

        foreach (string n in names)
        {
            if (string.IsNullOrEmpty(n)) continue;

            ItemData itemData = ItemDatabase.instance.Get(n);

            if (itemData == null)
            {
                Debug.LogWarning("⚠ Item introuvable: " + n);
                continue;
            }

            InventoryItem item = new InventoryItem();
            item.itemName = itemData.itemName;
            item.icon = itemData.icon;
            item.prefab = itemData.prefab;

            items.Add(item);
        }

        Debug.Log("📥 Inventory restauré OK = " + items.Count);
    }
    public void RestoreInventoryFromServer(string data)
    {
        StartCoroutine(RestoreInventoryCoroutine(data));
    }

    IEnumerator RestoreInventoryCoroutine(string data)
    {
        while (ItemDatabase.instance == null) yield return null;

        items.Clear();
        waterDropCount = 0;
        h2oCount = 0;

        if (string.IsNullOrEmpty(data)) yield break;

        string[] names = data.Split(';');

        foreach (string n in names)
        {
            if (string.IsNullOrEmpty(n)) continue;

            // Cas spécial WaterDrop
            if (n == "WaterDrop")
            {
                if (waterDropCount == 0)
                {
                    items.Add(waterDropItem);
                    waterDropCount++;
                }
                continue;
            }

            ItemData itemData = ItemDatabase.instance.Get(n);
            if (itemData == null)
            {
                Debug.LogWarning("⚠ Item introuvable: " + n);
                continue;
            }

            InventoryItem item = new InventoryItem();
            item.itemName = itemData.itemName;
            item.icon = itemData.icon;
            item.prefab = itemData.prefab;
            items.Add(item);

            if (n == "H2O") h2oCount++;
        }

        Debug.Log("📥 Inventory restauré depuis serveur = " + items.Count + " items");
    }
}