using UnityEngine;

public class ItemData : MonoBehaviour
{
    public string itemName;
    public Sprite icon;
    public GameObject prefab;
}
[System.Serializable]
public class InventoryResponse
{
    public string[] inventory;
}