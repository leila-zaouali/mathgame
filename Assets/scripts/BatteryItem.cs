using UnityEngine;

public class BatteryItem : MonoBehaviour
{
    public float voltage = 1.5f;

    public Vector3 startPosition;
    public Quaternion startRotation;
    public Transform startParent;

    public Vector3 defaultScale;

    public bool isInUse = false;

    void Start()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;
        startParent = transform.parent;

        defaultScale = transform.localScale;
    }

    public void ResetPosition()
    {
        transform.SetParent(startParent);
        transform.position = startPosition;
        transform.rotation = startRotation;

        ResetScale();

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }

    public void ResetScale()
    {
        transform.localScale = defaultScale;
    }
}