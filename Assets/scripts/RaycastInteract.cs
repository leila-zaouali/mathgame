using UnityEngine;
using static UnityEngine.InputSystem.Controls.AxisControl;

public class RaycastInteract : MonoBehaviour
{
    public float distance = 5f;
    public LayerMask interactLayer;

    public Transform handPoint;

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

        HandleInteraction(hit);
    }

    void HandleInteraction(RaycastHit hit)
    {
        if (GameInput.instance == null) return;

        // ===================== COLLECTIBLE =====================
        if (hit.collider.CompareTag("Collectible") &&
            GameInput.instance.InteractPressed())
        {
            Transform root = hit.collider.transform.root;

            ItemData data = root.GetComponent<ItemData>();

            if (data != null)
            {
                Inventory.instance.AddItem(data.itemName, data.icon, data.prefab);
                Destroy(root.gameObject);
            }

            return;
        }
        if (hit.collider.GetComponentInParent<battreieslot>() &&
      GameInput.instance.InteractPressed())
        {
            battreieslot slot =
                hit.collider.GetComponentInParent<battreieslot>();

            // 👉 si j’ai une batterie en main → je la mets dans slot
            if (currentBattery != null)
            {
                slot.InsertBattery(currentBattery.gameObject);
                currentBattery = null;
            }
            // 👉 sinon je reprends la batterie du slot
            else
            {
                GameObject batteryObj = slot.RemoveBattery();

                if (batteryObj != null)
                {
                    currentBattery = batteryObj.GetComponent<BatteryItem>();

                    Rigidbody rb = batteryObj.GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        rb.isKinematic = true;
                        rb.linearVelocity = Vector3.zero;
                        rb.angularVelocity = Vector3.zero;
                    }

                    batteryObj.transform.SetParent(handPoint);
                    batteryObj.transform.localPosition = Vector3.zero;
                    batteryObj.transform.localRotation = Quaternion.identity;
                }
            }

            return;
        }
        // ===================== BATTERY =====================
        if (hit.collider.CompareTag("Battery") &&
            GameInput.instance.InteractPressed())
        {
            Voltmeter vm = Object.FindFirstObjectByType<Voltmeter>();

            if (vm != null && vm.HasBattery())
            {
                Debug.Log("⚠ Voltmètre occupé");
                return;
            }

            BatteryItem newBattery = hit.collider.GetComponentInParent<BatteryItem>();
            if (newBattery == null) return;

            if (newBattery.isInUse) return;

            if (currentBattery != null)
                currentBattery.ResetPosition();

            currentBattery = newBattery;

            Rigidbody rb = currentBattery.GetComponent<Rigidbody>();
            if (rb != null)
                rb.isKinematic = true;

            currentBattery.transform.SetParent(handPoint);
            currentBattery.transform.localPosition = Vector3.zero;
            currentBattery.transform.localRotation = Quaternion.identity;

            return;
        }

        // ===================== VOLTMETER =====================
        if (hit.collider.CompareTag("Voltmeter") &&
            GameInput.instance.InteractPressed())
        {
            Voltmeter vm = hit.collider.GetComponent<Voltmeter>();

            if (vm == null) return;

            if (currentBattery != null)
            {
                vm.InsertBattery(currentBattery.gameObject);
                currentBattery = null;
            }
            else
            {
                GameObject batteryObj = vm.RemoveBattery();

                if (batteryObj != null)
                {
                    currentBattery = batteryObj.GetComponent<BatteryItem>();

                    Rigidbody rb = batteryObj.GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        rb.isKinematic = false;
                        rb.linearVelocity = Vector3.zero;
                        rb.angularVelocity = Vector3.zero;
                    }

                    batteryObj.transform.SetParent(handPoint);
                    batteryObj.transform.localPosition = Vector3.zero;
                    batteryObj.transform.localRotation = Quaternion.identity;
                }
            }

            return;
        }

        // ===================== PUZZLE =====================
        if (hit.collider.CompareTag("CorrectD") &&
            GameInput.instance.InteractPressed() &&
            !puzzleSolved)
        {
            puzzleSolved = true;

            floorBlock.transform.position += Vector3.up * 1.5f;
            hiddenKey.SetActive(true);
        }
    }
}