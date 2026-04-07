using UnityEngine;

public class SunRandomizer : MonoBehaviour
{
    public Light sun;

    void Start()
    {
        // Angle bas pour garder une ombre longue
        float angleX = Random.Range(10f, 25f);

        // Direction aléatoire
        float angleY = Random.Range(0f, 360f);

        sun.transform.rotation = Quaternion.Euler(angleX, angleY, 0f);
    }
}
