using UnityEngine;

public class RaycastInteract : MonoBehaviour
{
    public float distance = 5f;
    public GameObject player;

    void Update()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.5f));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, distance))
        {
            // Vérifie si le collider touché a le tag "Collectible"
            if (hit.collider.CompareTag("Collectible"))
            {
                // Si le joueur appuie sur la touche E
                if (Input.GetKeyDown(KeyCode.E))
                {
                    // Remonter au parent pour prendre le groupe entier
                    Transform parent = hit.collider.transform.parent;
                    GameObject toCollect = parent != null ? parent.gameObject : hit.collider.gameObject;

                    Inventory inv = player.GetComponent<Inventory>();
                    if (inv != null)
                    {
                        inv.AddItem(toCollect.name); // ajoute le nom du parent ou de l’enfant
                        Destroy(toCollect); // supprime tout le groupe
                        Debug.Log("Objet ramassé : " + toCollect.name);
                    }
                }
            }
        }
    }
}