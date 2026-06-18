using UnityEngine;

public class Lampe : MonoBehaviour
{
    public Transform batteryPosition;
    public Light lampLight;

    public float requiredVoltage = 2f;

    private BatteryItem currentBattery;
    private bool switchOn = false;

    private bool scoreGiven = false;

    public bool HasBattery()
    {
        return currentBattery != null;
    }

    public void SetSwitch(bool state)
    {
        switchOn = state;
        UpdateLight();
    }

    public void InsertBattery(GameObject battery)
    {
        if (currentBattery != null) return;

        currentBattery = battery.GetComponent<BatteryItem>();

        Rigidbody rb = battery.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        battery.transform.SetParent(batteryPosition);
        battery.transform.localPosition = Vector3.zero;
        battery.transform.localRotation = Quaternion.identity;

        currentBattery.ResetScale();

        UpdateLight();
    }

    public GameObject RemoveBattery()
    {
        if (currentBattery == null) return null;

        GameObject battery = currentBattery.gameObject;
        currentBattery = null;

        battery.transform.SetParent(null);

        Rigidbody rb = battery.GetComponent<Rigidbody>();
        if (rb != null)
            rb.isKinematic = false;

        UpdateLight();

        return battery;
    }

    public bool IsActive()
    {
        return lampLight != null && lampLight.enabled;
    }
    void UpdateLight()
    {
        if (lampLight == null) return;
        bool hasEnergy =
            currentBattery != null &&
            Mathf.Approximately(currentBattery.voltage, requiredVoltage);
        bool shouldBeOn = hasEnergy && switchOn;

        // ✅ AJOUTE
        Debug.Log("💡 UpdateLight: hasEnergy=" + hasEnergy + " switchOn=" + switchOn + " shouldBeOn=" + shouldBeOn);

        lampLight.enabled = shouldBeOn;
        if (shouldBeOn && !scoreGiven)
        {
            scoreGiven = true;
            if (ScoreManager.instance != null)
                ScoreManager.instance.AddScore(50);
            Debug.Log("💡 Lampe ON");
        }
    }
    public void TurnOnSilent()
    {
        scoreGiven = true; // bloquer le score
        SetSwitch(true);
    }
    public void TurnOn()
    {
        SetSwitch(true);
    }

    public void TurnOff()
    {
        SetSwitch(false);
    }
}