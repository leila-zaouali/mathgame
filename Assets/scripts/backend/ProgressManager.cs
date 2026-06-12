using UnityEngine;

public class ProgressManager : MonoBehaviour
{
    public static ProgressManager instance;

    public bool game1Completed;
    public bool lampPuzzleCompleted;

    // CHECKPOINT
    public string checkpointScene;
    public Vector3 checkpointPosition;
    public bool hasCheckpoint = false;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
        LoadCheckpoint(); // 🔥 IMPORTANT

        Debug.Log("HAS CHECKPOINT = " + hasCheckpoint);
        Debug.Log("CHECKPOINT POS = " + checkpointPosition);
    }
    void LoadCheckpoint()
    {
        if (PlayerPrefs.HasKey("cp_x"))
        {
            checkpointPosition = new Vector3(
                PlayerPrefs.GetFloat("cp_x"),
                PlayerPrefs.GetFloat("cp_y"),
                PlayerPrefs.GetFloat("cp_z")
            );

            hasCheckpoint = true;

            Debug.Log("📥 Checkpoint chargé depuis PlayerPrefs");
        }
    }
}