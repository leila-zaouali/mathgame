//using System.Collections.Generic;
//using UnityEngine;

//public class ItemDatabase : MonoBehaviour
//{
//    public static ItemDatabase instance;

//    public List<ItemData> items;

//    void Awake()
//    {
//        if (instance != null && instance != this)
//        {
//            Destroy(gameObject);
//            return;
//        }

//        instance = this;
//        DontDestroyOnLoad(gameObject);

//        Debug.Log("📦 ItemDatabase READY");
//    }

//    public ItemData Get(string name)
//    {
//        return items.Find(i => i.itemName == name);
//    }
//}