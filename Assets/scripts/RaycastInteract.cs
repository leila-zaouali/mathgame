using UnityEngine;

public class RaycastInteract : MonoBehaviour
{
    public float distance = 5f;
    public LayerMask interactLayer;
    public Transform handPoint;
    public GameObject hiddenKey;
    public GameObject floorBlock;

    private bool puzzleSolved = false;
    private BatteryItem currentBattery;

    public bool HasBattery()
    {
        return currentBattery != null;
    }

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

        // ================= COLLECTIBLE =================
        if (hit.collider.CompareTag("Collectible") &&
            GameInput.instance.InteractPressed())
        {
            WorldItem item = hit.collider.GetComponentInParent<WorldItem>();
            if (item != null)
            {
                Inventory.instance.AddItem(item.itemName, item.icon, item.prefab);
                Destroy(item.gameObject);
            }
            return;
        }

        // ================= BATTERY SLOT =================
        // ================= BATTERY SLOT =================
        if (hit.collider.GetComponentInParent<battreieslot>() &&
            GameInput.instance.InteractPressed())
        {
            battreieslot slot = hit.collider.GetComponentInParent<battreieslot>();

            // ✅ Si pile non trackée dans handPoint, la tracker
            if (currentBattery == null && handPoint.childCount > 0)
            {
                currentBattery = handPoint.GetChild(0).GetComponent<BatteryItem>();
            }

            if (currentBattery != null)
            {
                if (!slot.HasBattery())
                {
                    slot.InsertBattery(currentBattery.gameObject);
                    currentBattery = null;
                }
                else
                    Debug.Log("⚠ Slot déjà occupé");
            }
            else
            {
                GameObject batteryObj = slot.RemoveBattery();
                if (batteryObj != null)
                {
                    currentBattery = batteryObj.GetComponent<BatteryItem>();
                    Rigidbody rb = batteryObj.GetComponent<Rigidbody>();
                    if (rb != null) { rb.isKinematic = true; rb.linearVelocity = Vector3.zero; rb.angularVelocity = Vector3.zero; }
                    batteryObj.transform.SetParent(handPoint);
                    batteryObj.transform.localPosition = Vector3.zero;
                    batteryObj.transform.localRotation = Quaternion.identity;
                }
            }
            return;
        }

        // ================= BATTERY PICK =================
        if (hit.collider.CompareTag("Battery") &&
         GameInput.instance.InteractPressed())
        {
            if (currentBattery != null)
            {
                Debug.Log("⚠ Vous avez déjà une batterie en main");
                return;
            }

            BatteryItem newBattery = hit.collider.GetComponentInParent<BatteryItem>();
            if (newBattery == null) return;
            if (newBattery.isInUse) return;

            // ✅ Remettre toute pile non trackée dans handPoint à sa position initiale
            foreach (Transform child in handPoint)
            {
                BatteryItem b = child.GetComponent<BatteryItem>();
                if (b != null)
                    b.ResetPosition();
            }

            currentBattery = newBattery;
            Rigidbody rb = currentBattery.GetComponent<Rigidbody>();
            if (rb != null)
                rb.isKinematic = true;

            currentBattery.transform.SetParent(handPoint);
            currentBattery.transform.localPosition = Vector3.zero;
            currentBattery.transform.localRotation = Quaternion.identity;
            return;
        }
        // ================= VOLTMETER =================
        if (hit.collider.CompareTag("Voltmeter") &&
            GameInput.instance.InteractPressed())
        {
            Voltmeter vm = hit.collider.GetComponent<Voltmeter>();
            if (vm == null) return;
            if (currentBattery == null && handPoint.childCount > 0)
            {
                currentBattery = handPoint.GetChild(0).GetComponent<BatteryItem>();
            }

            if (currentBattery != null)
            {
                vm.InsertBattery(currentBattery.gameObject);
                currentBattery = null;
            }
            else
            {
                // ✅ Bloquer si main occupée
                if (handPoint.childCount > 0)
                {
                    Debug.Log("⚠ Main occupée");
                    return;
                }
                GameObject batteryObj = vm.RemoveBattery();
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

        // ================= LAMPE =================
        if (hit.collider.CompareTag("Lampe") &&
            GameInput.instance.InteractPressed())
        {
            Lampe lampe = hit.collider.GetComponent<Lampe>();
            if (lampe == null) return;

            // ✅ Tracker pile non trackée
            if (currentBattery == null && handPoint.childCount > 0)
                currentBattery = handPoint.GetChild(0).GetComponent<BatteryItem>();

            if (currentBattery != null)
            {
                // ✅ Insérer seulement si lampe vide
                if (!lampe.HasBattery())
                {
                    lampe.InsertBattery(currentBattery.gameObject);
                    currentBattery = null;
                }
                else
                {
                    Debug.Log("⚠ Lampe déjà occupée");
                }
            }
            else
            {
                GameObject batteryObj = lampe.RemoveBattery();
                if (batteryObj != null)
                {
                    currentBattery = batteryObj.GetComponent<BatteryItem>();
                    Rigidbody rb = batteryObj.GetComponent<Rigidbody>();
                    if (rb != null) { rb.isKinematic = true; rb.linearVelocity = Vector3.zero; rb.angularVelocity = Vector3.zero; }
                    batteryObj.transform.SetParent(handPoint);
                    batteryObj.transform.localPosition = Vector3.zero;
                    batteryObj.transform.localRotation = Quaternion.identity;
                }
            }
            return;
        }

        // ================= LAMP SWITCH =================
        if (hit.collider.GetComponentInParent<LampSwitch>() &&
            GameInput.instance.InteractPressed())
        {
            LampSwitch sw = hit.collider.GetComponentInParent<LampSwitch>();
            if (sw != null)
                sw.Toggle();
            return;
        }

        // ================= PUZZLE =================
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