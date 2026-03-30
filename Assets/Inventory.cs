using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<string> items = new List<string>();

    public void AddItem(string itemName)
    {
        items.Add(itemName);
        Debug.Log("Objet ajouté : " + itemName);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            Debug.Log("=== INVENTAIRE ===");

            foreach (string item in items)
            {
                Debug.Log(item);
            }
        }
    }
}