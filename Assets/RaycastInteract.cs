using UnityEngine;

public class RaycastInteract : MonoBehaviour
{
    public float distance = 5f;
    public GameObject player;
    public LayerMask interactLayer;

    // 🔑 Puzzle Thalès
    public GameObject hiddenKey;
    public GameObject floorBlock;

    private bool puzzleSolved = false;

    void Update()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, distance, interactLayer))
        {
            // =========================
            // 🧪 RAMASSER MOLÉCULE
            // =========================
            if (hit.collider.CompareTag("Collectible"))
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
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

            // =========================
            // 📏 PUZZLE THALÈS
            // =========================
            if (hit.collider.CompareTag("CorrectD"))
            {
                if (Input.GetKeyDown(KeyCode.E) && !puzzleSolved)
                {
                    puzzleSolved = true;

                    Debug.Log("✔ Bonne position D");

                    // 🔥 le carré monte
                    floorBlock.transform.position += Vector3.up * 1.5f;

                    // 🔑 afficher clé
                    hiddenKey.SetActive(true);
                }
            }
        }
    }
}