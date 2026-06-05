using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    public int currentScore;

    private APIManager api;
    private bool isSending = false;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        api = FindFirstObjectByType<APIManager>();
    }

    public void SetScore(int value)
    {
        currentScore = value;
        Debug.Log("📥 SCORE SET = " + currentScore);
    }

    // ⭐ AJOUT SCORE NORMAL
    public void AddScore(int value, bool sync = true)
    {
        currentScore += value;
        Debug.Log("⭐ SCORE = " + currentScore);

        if (!sync) return;

        if (api == null || string.IsNullOrEmpty(PlayerSession.UserId))
        {
            Debug.Log("❌ API ou USER ID NULL");
            return;
        }

        if (!isSending)
            StartCoroutine(SendScore(value));
    }

    private System.Collections.IEnumerator SendScore(int value)
    {
        isSending = true;

        api.SaveScore(value);

        yield return new WaitForSeconds(0.2f);

        isSending = false;
    }

    // ⭐ BONUS FIN DE NIVEAU (NO DOUBLE CALL)
    public void AddLevelBonus()
    {
        Debug.Log("🏁 LEVEL COMPLETED +50");

        AddScore(50, true);
    }
}