using UnityEngine;

public class RaycastInteract : MonoBehaviour
{
    public float distance = 5f;
    public GameObject player;
    public LayerMask interactLayer;

    void Update()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, distance, interactLayer))
        {
            if (hit.collider.CompareTag("Collectible"))
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    // 🔥 prendre la MOLÉCULE (parent)
                    Transform parent = hit.collider.transform.root;

                    ItemData data = parent.GetComponent<ItemData>();

                    if (data != null)
                    {
                        Inventory.instance.AddItem(data.itemName, data.icon);
                        Destroy(parent.gameObject);
                        Debug.Log("Molécule ramassée : " + data.itemName);
                    }
                    else
                    {
                        Debug.Log("ItemData manquant sur le parent !");
                    }
                }
            }
        }
    }
}