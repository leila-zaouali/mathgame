using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject menuPanel; // ton Panel UI
    public Camera transitionCamera; // ✅ assigne dans l'Inspector


    void Update()
    {
        if (GameInput.instance != null && GameInput.instance.MenuPressed())
        {
            ToggleMenu();
        }
    }
    public void ToggleMenu()
    {
        menuPanel.SetActive(!menuPanel.activeSelf);

        if (menuPanel.activeSelf)
        {
            // Menu ouvert → activer la souris
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            // Menu fermé → reprendre le comportement normal
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    // =====================
    // BOUTON REJOUER
    // =====================
    public void OnReplay()
    {
        // ✅ Reset sur Firebase
        if (APIManager.instance != null)
            APIManager.instance.ResetProgress();

        // ✅ Reset ProgressManager
        // ✅ Reset progression locale
        if (ProgressManager.instance != null)
        {
            ProgressManager.instance.ClearCheckpoint();
            ProgressManager.instance.game1Completed = false;       // ✅ AJOUTE
            ProgressManager.instance.lampPuzzleCompleted = false;  // ✅ AJOUTE
        }

        // ✅ Vider inventaire
        if (Inventory.instance != null)
        {
            Inventory.instance.items.Clear();
            Inventory.instance.waterDropCount = 0;
            Inventory.instance.h2oCount = 0;
        }

        // ✅ Reset score
        if (ScoreManager.instance != null)
            ScoreManager.instance.SetScore(0);

        // ✅ Forcer intro1
        APIManager.progressLoaded = true;
        APIManager.nextScene = "intro1";

        SceneManager.LoadScene("LoadingScene");

        menuPanel.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // =====================
    // BOUTON DECONNEXION
    // =====================

    public void OnLogout()
    {
        // ✅ Activer la caméra de transition AVANT tout
        if (transitionCamera != null)
            transitionCamera.gameObject.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        PlayerSession.UserId = "";

        if (Inventory.instance != null)
        {
            Destroy(Inventory.instance.gameObject);
            Inventory.instance = null;
        }

        if (APIManager.instance != null)
        {
            Destroy(APIManager.instance.gameObject);
            APIManager.instance = null;
        }

        if (ScoreManager.instance != null)
        {
            Destroy(ScoreManager.instance.gameObject);
            ScoreManager.instance = null;
        }

        if (GameInput.instance != null)
        {
            Destroy(GameInput.instance.gameObject);
            GameInput.instance = null;
        }

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
            Destroy(player);

        SceneManager.LoadScene("LoginScene");
    }
    // =====================
    // RESET PROGRESSION
    // =====================
    void ResetAllProgress()
    {
        // Vider inventaire
        if (Inventory.instance != null)
        {
            Inventory.instance.items.Clear();
            Inventory.instance.waterDropCount = 0;
            Inventory.instance.h2oCount = 0;
        }

        // Reset score
        if (ScoreManager.instance != null)
            ScoreManager.instance.SetScore(0);

        // Reset sur le serveur
        if (APIManager.instance != null)
        {
            APIManager.instance.SaveScore(0);
            APIManager.instance.SaveInventory("");
            APIManager.instance.SaveProgress("game1", false);
            APIManager.instance.SaveProgress("lamp_puzzle", false);
            APIManager.instance.UpdateLevel();
        }

        // Vider PlayerPrefs checkpoint
        string id = PlayerSession.UserId;
        PlayerPrefs.DeleteKey(id + "_cp_active");
        PlayerPrefs.DeleteKey(id + "_cp_scene");
        PlayerPrefs.Save();
    }
}