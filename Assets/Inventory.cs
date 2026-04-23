using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<string> items = new List<string>();

    public int h2oCount = 0; // 👈 compteur H2O

    public void AddItem(string itemName)
    {
        items.Add(itemName);

        if (itemName == "H2O")
        {
            h2oCount++;
        }

        Debug.Log("Ajouté : " + itemName + " | H2O = " + h2oCount);
    }
}