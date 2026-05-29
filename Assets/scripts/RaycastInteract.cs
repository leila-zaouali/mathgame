using UnityEngine;

public class RaycastInteract : MonoBehaviour
{
    public float distance = 5f;
    public LayerMask interactLayer;

    public GameObject hiddenKey;
    public GameObject floorBlock;

    private bool puzzleSolved = false;
    private BatteryItem currentBattery;

    void Update()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;

        Debug.DrawRay(ray.origin, ray.direction * distance, Color.red);

        if (!Physics.Raycast(ray, out hit, distance, interactLayer))
            return;

        Debug.Log("HIT : " + hit.collider.name);

        HandleInteraction(hit);
    }

    void HandleInteraction(RaycastHit hit)
    {
        if (GameInput.instance == null) return;

        // ==================================================
        // 🧪 COLLECTIBLES
        // ==================================================
        if (hit.collider.CompareTag("Collectible") &&
            GameInput.instance.InteractPressed())
        {
            Transform root = hit.collider.transform.root;

            ItemData data = root.GetComponent<ItemData>();

            if (data != null)
            {
                Inventory.instance.AddItem(data.itemName, data.icon, data.prefab);
                Destroy(root.gameObject);
                Debug.Log("✔ Ramassé : " + data.itemName);
            }

            return;
        }

        // ==================================================
        // 🔋 BATTERIE
        // ==================================================
        if (hit.collider.CompareTag("Battery") &&
            GameInput.instance.InteractPressed())
        {
            BatteryItem newBattery =
                hit.collider.GetComponentInParent<BatteryItem>();

            if (newBattery == null) return;

            if (currentBattery != null && currentBattery != newBattery)
                currentBattery.ResetPosition();

            currentBattery = newBattery;

            Rigidbody rb = currentBattery.GetComponent<Rigidbody>();
            if (rb != null)
                rb.isKinematic = true;

            Debug.Log("🔋 Batterie sélectionnée : " + currentBattery.name);

            return;
        }

        // ==================================================
        // ⚡ VOLTMÈTRE
        // ==================================================
        if (hit.collider.CompareTag("Voltmeter") &&
            GameInput.instance.InteractPressed())
        {
            if (currentBattery == null) return;

            Voltmeter vm = hit.collider.GetComponent<Voltmeter>();

            if (vm != null)
            {
                vm.InsertBattery(currentBattery.gameObject);
                currentBattery = null;

                Debug.Log("⚡ Batterie insérée");
            }

            return;
        }

        // ==================================================
        // 📏 PUZZLE THALÈS
        // ==================================================
        if (hit.collider.CompareTag("CorrectD") &&
            GameInput.instance.InteractPressed() &&
            !puzzleSolved)
        {
            puzzleSolved = true;

            floorBlock.transform.position += Vector3.up * 1.5f;
            hiddenKey.SetActive(true);

            Debug.Log("✔ Puzzle résolu");
        }
    }
}