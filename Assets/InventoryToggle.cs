using UnityEngine;

public class InventoryToggle : MonoBehaviour
{
    public GameObject inventoryUI;

    private bool isOpen = false;

    void Start()
    {
        if (inventoryUI != null)
            inventoryUI.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            isOpen = !isOpen;

            if (inventoryUI != null)
                inventoryUI.SetActive(isOpen);
        }
    }
}