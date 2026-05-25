using UnityEngine;

public class InventoryToggle : MonoBehaviour
{
    public GameObject inventoryUI;

    private bool isOpen = false;

    void Start()
    {
        inventoryUI.SetActive(false);
    }

    void Update()
    {
        
       
            if (GameInput.instance.InventoryPressed())
            {
                isOpen = !isOpen;

                inventoryUI.SetActive(isOpen);

                Cursor.visible = isOpen;

                Cursor.lockState = isOpen
                    ? CursorLockMode.None
                    : CursorLockMode.Locked;
            }
        }
    
}