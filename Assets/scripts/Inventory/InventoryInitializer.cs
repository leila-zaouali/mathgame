using System.Collections;
using UnityEngine;

public class InventoryInitializer : MonoBehaviour
{
    IEnumerator Start()
    {
        yield return null;

        while (Inventory.instance == null)
            yield return null;

        if (APIManager.restoreInventory)
        {
            Inventory.instance.SetWaterDropFromSave();

            Debug.Log("💧 INVENTAIRE RESTAURÉ DANS SCÈNE");

            APIManager.restoreInventory = false;
        }
    }
}