using UnityEngine;
public class LampSwitch : MonoBehaviour
{
    public Lampe lampe;
    public Transform switchHandle;
    [Header("Rotation sur X")]
    public float offAngleX = 0f;
    public float onAngleX = 30f;
    private bool isOn = false;

    public void Toggle()
    {
        isOn = !isOn;
        if (lampe != null)
            lampe.SetSwitch(isOn);
        if (switchHandle != null)
        {
            Vector3 rot = switchHandle.localEulerAngles;
            rot.x = isOn ? onAngleX : offAngleX;
            switchHandle.localEulerAngles = rot;
        }
        Debug.Log("🔘 Switch = " + isOn);
    }

    // ✅ AJOUTE
    public void SetOn()
    {
        isOn = true;
        if (switchHandle != null)
        {
            Vector3 rot = switchHandle.localEulerAngles;
            rot.x = onAngleX;
            switchHandle.localEulerAngles = rot;
        }
    }
}