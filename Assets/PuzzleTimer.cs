using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PuzzleTimer : MonoBehaviour
{
    [Header("Configuration du temps limite")]
    [Tooltip("Temps en secondes accorde au joueur pour resoudre CE puzzle")]
    public float timeLimit = 120f;

    [Tooltip("Le timer demarre automatiquement au lancement de la scene")]
    public bool autoStart = true;

    [Header("Verification de progression")]
    [Tooltip("Si VRAI, le script verifie IsPuzzleAlreadyCompleted() avant de demarrer le timer")]
    public bool checkIfAlreadyCompleted = true;

    [Header("Optionnel - UI")]
    [Tooltip("Texte UI (TextMeshPro) qui affiche le temps restant. Laisse vide si non utilise.")]
    public TMPro.TextMeshProUGUI timerText;

    private float timeRemaining;
    private bool isRunning = false;
    private bool puzzleCompleted = false;

    void Start()
    {
        timeRemaining = timeLimit;

        if (checkIfAlreadyCompleted)
        {
            StartCoroutine(WaitForProgressThenCheck());
        }
        else if (autoStart)
        {
            StartTimer();
        }
    }

    IEnumerator WaitForProgressThenCheck()
    {
        // ✅ On attend que la progression soit chargee depuis le serveur
        // avant de decider si le timer doit demarrer ou non.
        while (!APIManager.progressLoaded)
        {
            yield return null;
        }

        if (IsPuzzleAlreadyCompleted())
        {
            // Le puzzle a deja ete resolu auparavant -> pas besoin de chrono
            puzzleCompleted = true;
            isRunning = false;

            if (timerText != null)
                timerText.gameObject.SetActive(false);

            Debug.Log("✅ Puzzle deja complete -> timer desactive pour cette scene.");
        }
        else if (autoStart)
        {
            StartTimer();
        }
    }

    // ✅ Verifie le bon booleen de ProgressManager selon la scene actuelle.
    // Pour l'instant seuls game1Completed et lampPuzzleCompleted existent
    // (cf. ProgressData : game1 + lamp_puzzle).
    // Quand tu ajouteras un nouveau puzzle :
    //   1. Ajoute le champ dans ProgressData (backend + Unity)
    //   2. Ajoute le bool correspondant dans ProgressManager
    //   3. Ajoute un "case" ici avec le nom exact de la scene
    private bool IsPuzzleAlreadyCompleted()
    {
        if (ProgressManager.instance == null) return false;

        switch (SceneManager.GetActiveScene().name)
        {
            case "1game":
                return ProgressManager.instance.game1Completed;

            case "LampPuzzleScene":
                return ProgressManager.instance.lampPuzzleCompleted;

            // case "Puzzle3Scene":
            //     return ProgressManager.instance.puzzle3Completed;

            default:
                return false;
        }
    }

    void Update()
    {
        if (!isRunning || puzzleCompleted) return;

        timeRemaining -= Time.deltaTime;

        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(timeRemaining / 60f);
            int seconds = Mathf.FloorToInt(timeRemaining % 60f);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }

        if (timeRemaining <= 0f)
        {
            timeRemaining = 0f;
            isRunning = false;
            OnTimeUp();
        }
    }

    public void StartTimer()
    {
        timeRemaining = timeLimit;
        isRunning = true;
        puzzleCompleted = false;
    }

    public void PauseTimer()
    {
        isRunning = false;
    }

    public void ResumeTimer()
    {
        isRunning = true;
    }

    // ✅ A appeler depuis ton PuzzleManager quand le joueur reussit le puzzle
    public void PuzzleSolved()
    {
        puzzleCompleted = true;
        isRunning = false;
        Debug.Log("✅ Puzzle resolu avant la fin du timer.");
    }

    private void OnTimeUp()
    {
        Debug.Log("⏰ Temps ecoule ! Retour au checkpoint : " +
            (ProgressManager.instance != null ? ProgressManager.instance.checkpointScene : "AUCUN"));

        StartCoroutine(ReturnToCheckpoint());
    }

    private IEnumerator ReturnToCheckpoint()
    {
        // Petite pause pour laisser un feedback visuel/sonore si tu veux en ajouter
        yield return new WaitForSeconds(0.5f);

        if (ProgressManager.instance != null && ProgressManager.instance.hasCheckpoint)
        {
            string sceneToLoad = ProgressManager.instance.checkpointScene;
            Debug.Log("🔄 Rechargement de la scene checkpoint : " + sceneToLoad);
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            // Pas de checkpoint enregistre -> on recharge la scene actuelle par securite
            Debug.LogWarning("⚠️ Aucun checkpoint trouve, rechargement de la scene actuelle.");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}