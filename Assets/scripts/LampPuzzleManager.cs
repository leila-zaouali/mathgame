using UnityEngine;

public class LampPuzzleManager : MonoBehaviour
{
    public Lampe lamp1;
    public Lampe lamp2;
    public Lampe lamp3;

    public GameObject wallImage;

    private bool puzzleCompleted = false;

    void Start()
    {
        if (wallImage != null)
            wallImage.SetActive(false);

        if (ProgressManager.instance != null &&
            ProgressManager.instance.lampPuzzleCompleted)
        {
            wallImage.SetActive(true);

            Debug.Log("Puzzle lampe déjà terminé");
        }
    }

    void Update()
    {
        if (puzzleCompleted) return;

        if (lamp1 == null || lamp2 == null || lamp3 == null) return;

        bool allOn =
            lamp1.IsActive() &&
            lamp2.IsActive() &&
            lamp3.IsActive();

        if (!allOn) return;

        puzzleCompleted = true;

        Debug.Log("🏁 Puzzle lampes terminé");

        if (wallImage != null)
            wallImage.SetActive(true);

        // ⭐ PROGRESSION BACKEND
        if (APIManager.instance != null)
        {
            APIManager.instance.UpdateLevel();

            APIManager.instance.SaveProgress("lamp_puzzle", true);
        }

        // ⭐ SCORE BONUS GLOBAL (optionnel)
        if (ScoreManager.instance != null)
        {
            ScoreManager.instance.AddLevelBonus();
        }

        // ⭐ NOTIF GAME PROGRESS MANAGER (option clean)
        //if (GameProgressManager.instance != null)
        //{
        //    GameProgressManager.instance.CompleteLevel("lamp_puzzle");
        //}
    }
}