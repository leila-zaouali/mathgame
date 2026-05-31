using UnityEngine;

public class BatteryItem : MonoBehaviour
{
    public float voltage = 1.5f;

    public Vector3 startPosition;
    public Quaternion startRotation;
    public Transform startParent;

    public bool isInUse = false;

    void Start()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;
        startParent = transform.parent;
    }

    public void ResetPosition()
    {
        if (isInUse) return; // 🔒 bloque si utilisée

        transform.SetParent(startParent);
        transform.position = startPosition;
        transform.rotation = startRotation;

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        Debug.Log("🔄 Batterie reset");
    }
}