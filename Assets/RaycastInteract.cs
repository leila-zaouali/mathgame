using UnityEngine;

public class RaycastInteract : MonoBehaviour
{
    public float distance = 5f;

    void Update()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, distance))
        {
            Debug.Log("Touché : " + hit.collider.name);

            if (hit.collider.CompareTag("Collectible"))
            {
                Debug.Log("C'est collectible");

                if (Input.GetKeyDown(KeyCode.E))
                {
                    PickupItem item = hit.collider.GetComponent<PickupItem>();

                    if (item != null)
                    {
                        item.PickUp(GameObject.FindGameObjectWithTag("Player"));
                    }
                }
            }
        }
    }
}