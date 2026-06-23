using UnityEngine;

public class WaterPickup : MonoBehaviour
{
    private bool picked = false;

    private void OnTriggerEnter(Collider other)
    {
        if (picked) return;

        if (other.CompareTag("Player"))
        {
            picked = true;

            if (Inventory.instance != null)
            {
                Inventory.instance.AddItem("H2O", null, null);
            }

            Destroy(gameObject);
        }
    }
}