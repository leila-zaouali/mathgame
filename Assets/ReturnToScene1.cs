using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToScene1 : MonoBehaviour
{
    public int requiredH2O = 3;
    public string scene1Name = "intro"; // ⚠️ ton vrai nom

    private Inventory inventory;
    private bool hasLoaded = false;

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            inventory = Inventory.instance; // ✔ plus fiable
        }
    }

    void Update()
    {
        if (hasLoaded) return;

        if (inventory != null)
        {
            if (inventory.h2oCount >= requiredH2O)
            {
                hasLoaded = true; // ✔ empêche boucle
                SceneManager.LoadScene(scene1Name);
            }
        }
    }
}