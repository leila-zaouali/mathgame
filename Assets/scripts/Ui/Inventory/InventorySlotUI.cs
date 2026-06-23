using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour, IPointerClickHandler
{
    public InventoryItem item;
    public Image icon;

    public void Setup(InventoryItem newItem)
    {
        item = newItem;
        icon.sprite = item.icon;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        InventorySelector.instance.SelectItem(item);
        Debug.Log("Sélectionné : " + item.itemName);
    }
}