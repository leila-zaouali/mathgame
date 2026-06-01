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

        // 🔥 update lampe
        if (lampe != null)
            lampe.SetSwitch(isOn);

        // 🔥 rotation switch sur X
        if (switchHandle != null)
        {
            Vector3 rot = switchHandle.localEulerAngles;
            rot.x = isOn ? onAngleX : offAngleX;
            switchHandle.localEulerAngles = rot;
        }

        Debug.Log("🔘 Switch = " + isOn);
    }
}