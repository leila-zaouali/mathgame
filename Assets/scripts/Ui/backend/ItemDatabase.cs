using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    public static ItemDatabase instance;

    public List<ItemData> items;

    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);

    }

    public ItemData Get(string name)
    {
        return items.Find(i => i.itemName == name);
    }
}