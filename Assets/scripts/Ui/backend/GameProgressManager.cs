using UnityEngine;

public class GameProgressManager : MonoBehaviour
{
    public static GameProgressManager instance;

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

    public void CompleteLevel(string levelName)
    {
        Debug.Log("🏁 Niveau terminé : " + levelName);

        if (APIManager.instance == null)
        {
            Debug.Log("❌ APIManager NULL");
            return;
        }

        if (string.IsNullOrEmpty(PlayerSession.UserId))
        {
            Debug.Log("❌ UserId NULL");
            return;
        }

        APIManager.instance.SaveProgress(levelName, true);
        APIManager.instance.UpdateLevel();

    }
}