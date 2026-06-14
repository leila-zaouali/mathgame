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

    public void LoadCheckpoint()
    {
        string id = PlayerSession.UserId;

        if (PlayerPrefs.GetInt(id + "_cp_active", 0) == 0)
        {
            hasCheckpoint = false;
            Debug.Log("📍 PAS DE CHECKPOINT POUR " + id);
            return;
        }

        checkpointScene = PlayerPrefs.GetString(id + "_cp_scene");

        checkpointPosition = new Vector3(
            PlayerPrefs.GetFloat(id + "_cp_x"),
            PlayerPrefs.GetFloat(id + "_cp_y"),
            PlayerPrefs.GetFloat(id + "_cp_z")
        );

        hasCheckpoint = true;

        Debug.Log("📥 CHECKPOINT CHARGÉ POUR " + id);
    }
}