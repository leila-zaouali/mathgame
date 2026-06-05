using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToScene1 : MonoBehaviour
{
    public int requiredH2O = 3;
    public string sceneName = "intro";

    private bool hasLoaded = false;

    void Update()
    {
        if (hasLoaded) return;
        if (Inventory.instance == null) return;

        if (Inventory.instance.totalH2OCollected >= requiredH2O)
        {
            hasLoaded = true;

            Debug.Log("✔ Niveau terminé");

            // ⭐ BONUS FINAL
            if (ScoreManager.instance != null)
                ScoreManager.instance.AddLevelBonus();

            // reset
            Inventory.instance.totalH2OCollected = 0;

            SceneManager.LoadScene(sceneName);
        }
    }
}