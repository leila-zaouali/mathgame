using UnityEngine;
using TMPro;

public class Voltmeter : MonoBehaviour
{
    public Transform batterySlot;
    public TextMeshProUGUI voltageText;

    private GameObject currentBattery;

    private string defaultText = "0 V";

    void Start()
    {
        ResetVoltmeter();
    }

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

        if (voltageText != null && data != null)
            voltageText.text = data.voltage + " V";

        Debug.Log("⚡ Batterie insérée");
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

        ResetVoltmeter();

        Debug.Log("🔋 Batterie retirée");

        return battery;
    }

    public bool HasBattery()
    {
        return currentBattery != null;
    }

    void ResetVoltmeter()
    {
        currentBattery = null;

        if (voltageText != null)
            voltageText.text = defaultText;

        Debug.Log("🔄 Voltmètre reset");
    }
}