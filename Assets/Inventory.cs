using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    public List<Sprite> items = new List<Sprite>();

    public int h2oCount = 0;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void AddItem(string itemName, Sprite icon)
    {
        items.Add(icon);

        if (itemName == "H2O")
        {
            h2oCount++;
        }

        Debug.Log("Ajouté : " + itemName);
    }

}