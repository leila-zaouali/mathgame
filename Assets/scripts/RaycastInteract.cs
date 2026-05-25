using UnityEngine;

public class RaycastInteract : MonoBehaviour
{
    public float distance = 5f;
    public LayerMask interactLayer;

    public GameObject hiddenKey;
    public GameObject floorBlock;

    private bool puzzleSolved = false;

    void Update()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, distance, interactLayer))
        {
            if (hit.collider.CompareTag("Collectible"))
            {
                if (GameInput.instance.InteractPressed())
                {
                    Transform root = hit.collider.transform.root;

                    ItemData data = root.GetComponent<ItemData>();

                    if (data != null)
                    {
                        // 📦 ajouter à l'inventaire
                        Inventory.instance.AddItem(
                            data.itemName,
                            data.icon,
                            data.prefab
                        );

                        // 💥 disparaît de la scène
                        Destroy(root.gameObject);

                        Debug.Log("✔ Ramassé : " + data.itemName);
                    }
                    else
                    {
                        Debug.Log("❌ ItemData manquant !");
                    }
                }
            }

            if (hit.collider.CompareTag("CorrectD"))
            {
                if (GameInput.instance.InteractPressed() && !puzzleSolved)
                {
                    puzzleSolved = true;

                    floorBlock.transform.position += Vector3.up * 1.5f;
                    hiddenKey.SetActive(true);
                }
            }
        }
    }
}