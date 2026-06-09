using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToScene1 : MonoBehaviour
{
    public int requiredH2O = 3;
    public string sceneName = "intro";

    private bool hasLoaded = false;
    private bool alreadyProcessed = false;

   void Update()
{
    if (alreadyProcessed) return;
    if (Inventory.instance == null) return;

    if (Inventory.instance.totalH2OCollected >= requiredH2O)
    {
        alreadyProcessed = true;

        Debug.Log("✔ Game1 terminé");

        // BONUS UNE SEULE FOIS
        if (ScoreManager.instance != null)
            ScoreManager.instance.AddLevelBonus();

        if (APIManager.instance != null)
        {
            APIManager.instance.SaveProgress("game1", true);
            APIManager.instance.UpdateLevel();
        }

        Inventory.instance.totalH2OCollected = 0;

        SceneManager.LoadScene(sceneName);
    }
}
}