using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;
    public Sprite h2oIcon;


    public List<Sprite> items = new List<Sprite>();

    public int h2oCount = 0;                // pour craft
    public int totalH2OCollected = 0;       // pour progression (NE JAMAIS RESET)

    public int waterDropCount = 0;

    public Sprite waterDropIcon;

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

    public void AddItem(string itemName, Sprite icon)
    {
        items.Add(icon);

        if (itemName == "H2O")
        {
            h2oCount++;
            totalH2OCollected++;

            Debug.Log("H2O +1 => " + h2oCount);

            // 💧 transformation
            if (h2oCount >= 3)
            {
                h2oCount -= 3;
                waterDropCount++;

                Debug.Log("💧 3 H2O → 1 goutte");
            }
        }
    }
}