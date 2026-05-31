using UnityEngine;

public class battreieslot : MonoBehaviour
{
    public Transform batterySlot;
    public Light lampLight;

    private GameObject currentBattery;

    public void InsertBattery(GameObject battery)
    {
        if (battery == null) return;
        if (currentBattery != null) return;

        currentBattery = battery;

        BatteryItem data = battery.GetComponent<BatteryItem>();
        if (data != null)
            data.isInUse = true;

        Rigidbody rb = battery.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        battery.transform.SetParent(batterySlot);
        battery.transform.localPosition = Vector3.zero;
        battery.transform.localRotation = Quaternion.identity;
        battery.transform.localScale = Vector3.one;

        if (lampLight != null)
            lampLight.enabled = true;

        Debug.Log("💡 Batterie dans slot");
    }

    public GameObject RemoveBattery()
    {
        if (currentBattery == null)
            return null;

        GameObject battery = currentBattery;
        currentBattery = null;

        BatteryItem data = battery.GetComponent<BatteryItem>();
        if (data != null)
            data.isInUse = false;

        battery.transform.SetParent(null);

        Rigidbody rb = battery.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        if (lampLight != null)
            lampLight.enabled = false;

        Debug.Log("🔋 Slot vidé");

        return battery;
    }

    public bool HasBattery()
    {
        return currentBattery != null;
    }
}