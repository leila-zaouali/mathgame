using UnityEngine;
using TMPro;

public class Voltmeter : MonoBehaviour
{
    public Transform batterySlot;
    public TMP_Text voltageText;

    private GameObject currentBattery;
    public void Start()
    {
        voltageText.text = "0 V";
    }
    public void InsertBattery(GameObject battery)
    {
        if (currentBattery != null) return;

        currentBattery = battery;

        Rigidbody rb = battery.GetComponent<Rigidbody>();
        if (rb != null)
            rb.isKinematic = true;

        battery.transform.SetParent(batterySlot);

        battery.transform.localPosition = Vector3.zero;
        battery.transform.localRotation = Quaternion.identity;
        BatteryItem item = battery.GetComponent<BatteryItem>();

        if (item != null)
        {
            voltageText.text = item.voltage + " V";
            Debug.Log("⚡ Voltage : " + item.voltage);
        }
        else
        {
            voltageText.text = "0 V";
        }
    }
}