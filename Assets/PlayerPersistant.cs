using UnityEngine;

public class PlayerPersistant : MonoBehaviour
{
    private static PlayerPersistant instance;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject); // supprime doublon
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}