using UnityEngine;
using UnityEngine.SceneManagement;

public class WaterPuzzleManager : MonoBehaviour
{
    public GameObject waterUI; // image ou objet visuel comme wallImage
    public string returnScene = "intro";

    private bool completed = false;

    void Start()
    {
        if (ProgressManager.instance != null &&
            ProgressManager.instance.game1Completed)
        {
            completed = true;

            Debug.Log("💧 Puzzle eau déjà terminé");

            if (waterUI != null)
                waterUI.SetActive(true);

            // donner la goutte directement
            if (Inventory.instance != null)
            {
                Inventory.instance.totalH2OCollected = 3;
            }

            // retour automatique scène 1
            SceneManager.LoadScene(returnScene);
        }
        if (ProgressManager.instance != null && ProgressManager.instance.game1Completed)
        {
            Debug.Log("💧 Puzzle eau déjà terminé - skip");

            // ⚠️ IMPORTANT : empêcher toute logique de score / level
            enabled = false;
            return;
        }
    }

    public void CompleteWaterPuzzle()
    {
        if (completed) return;

        completed = true;

        Debug.Log("💧 Puzzle eau terminé");

        if (APIManager.instance != null)
        {
            APIManager.instance.SaveProgress("game1", true);
            APIManager.instance.UpdateLevel();
        }

        if (waterUI != null)
            waterUI.SetActive(true);

        if (Inventory.instance != null)
        {
            Inventory.instance.totalH2OCollected = 3;
        }

        SceneManager.LoadScene(returnScene);
    }
}