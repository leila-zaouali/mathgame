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
        // ❌ Plus de LoadCheckpoint ici — c'est Firebase qui gère
    }

    // ✅ Appelé par APIManager après GetProgress
    public void SetCheckpoint(string scene, float x, float y, float z)
    {
        checkpointScene = scene;
        checkpointPosition = new Vector3(x, y, z);
        hasCheckpoint = true;
        Debug.Log("✅ CHECKPOINT SET: " + scene + " " + checkpointPosition);
    }

    public void ClearCheckpoint()
    {
        checkpointScene = "";
        checkpointPosition = Vector3.zero;
        hasCheckpoint = false;
        Debug.Log("🗑 CHECKPOINT CLEARED");
    }
}