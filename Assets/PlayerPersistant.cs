using UnityEngine;

public class PlayerPersistant : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}