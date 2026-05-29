using UnityEngine;

public class BatteryItem : MonoBehaviour
{
    public float voltage = 1.5f;
    public Vector3 startPosition;
    public Quaternion startRotation;
    public Transform startParent;

    void Start()
    {
        // 🔥 sauvegarde position initiale
        startPosition = transform.position;
        startRotation = transform.rotation;
        startParent = transform.parent;
    }

    public void ResetPosition()
    {
        transform.SetParent(startParent);
        transform.position = startPosition;
        transform.rotation = startRotation;

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
            rb.isKinematic = false;
    }
}