using UnityEngine;

public class ProgressManager : MonoBehaviour
{
    public static ProgressManager instance;

    public bool game1Completed;
    public bool lampPuzzleCompleted;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}