using UnityEngine;

public class LampPuzzleManager : MonoBehaviour
{
    public Lampe lamp1;
    public Lampe lamp2;
    public Lampe lamp3;

    public GameObject wallImage;

    void Start()
    {
        if (wallImage != null)
            wallImage.SetActive(false);
    }

    void Update()
    {
        if (lamp1 == null || lamp2 == null || lamp3 == null) return;

        bool allOn =
            lamp1.IsActive() &&
            lamp2.IsActive() &&
            lamp3.IsActive();

        if (wallImage != null)
            wallImage.SetActive(allOn);
    }
}