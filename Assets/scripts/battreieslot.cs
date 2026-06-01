using UnityEngine;

public class battreieslot : MonoBehaviour
{
    public Transform batterySlot;

    protected GameObject currentBattery;

    public virtual void InsertBattery(GameObject battery)
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

        Debug.Log("🔋 Batterie placée dans le slot");
    }

    public virtual GameObject RemoveBattery()
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

        Debug.Log("🔋 Batterie retirée du slot");

        return battery;
    }

    public bool HasBattery()
    {
        return currentBattery != null;
    }
}