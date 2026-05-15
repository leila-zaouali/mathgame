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

        Debug.Log("H2O CHECK = " + Inventory.instance.totalH2OCollected);

        if (Inventory.instance.totalH2OCollected >= requiredH2O)
        {
            hasLoaded = true;
            Debug.Log("✔ Retour scène déclenché");
            SceneManager.LoadScene(sceneName);
        }
    }
}