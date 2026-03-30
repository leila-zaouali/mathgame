using UnityEngine;

public class RaycastInteract : MonoBehaviour
{
    public float distance = 5f;
    public GameObject player;

    void Update()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, distance))
        {
            if (hit.collider.CompareTag("Collectible"))
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    Inventory inv = player.GetComponent<Inventory>();

                    if (inv != null)
                    {
                        inv.AddItem(hit.collider.gameObject.name);
                        Destroy(hit.collider.gameObject);
                    }
                }
            }
        }
    }
}