using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public string itemName;

    public void PickUp(GameObject player)
    {
        Inventory inv = player.GetComponent<Inventory>();

        if (inv != null)
        {
            inv.AddItem(itemName);
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Inventory script manquant sur le Player !");
        }
    }
}