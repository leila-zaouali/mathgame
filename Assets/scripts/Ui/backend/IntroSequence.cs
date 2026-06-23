using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable]
public class IntroPanel
{
    [Tooltip("Le panel UI (doit avoir un CanvasGroup attache)")]
    public CanvasGroup canvasGroup;

    [Tooltip("Combien de temps ce panel reste affiche a pleine opacite (en secondes)")]
    public float displayDuration = 3f;

    [Tooltip("Duree du fondu d'entree et de sortie (en secondes)")]
    public float fadeDuration = 1f;
}

public class IntroSequence : MonoBehaviour
{
    [Tooltip("Liste des panels a afficher dans l'ordre, avec leur duree et fondu")]
    public IntroPanel[] panels;

    [Tooltip("Nom de la scene a charger une fois la sequence terminee (ou skip)")]
    public string nextSceneName = "LoginScene";

    [Tooltip("Le bouton Skip (optionnel) - assigne-le dans l'Inspector")]
    public Button skipButton;

    private bool isSkipped = false;
    private Coroutine sequenceCoroutine;

    void Start()
    {
        // S'assure que tous les panels sont invisibles au depart
        foreach (var p in panels)
        {
            if (p.canvasGroup != null)
            {
                p.canvasGroup.alpha = 0f;
                p.canvasGroup.gameObject.SetActive(false);
            }
        }

        if (skipButton != null)
            skipButton.onClick.AddListener(SkipIntro);

        sequenceCoroutine = StartCoroutine(PlaySequence());
    }

    public void SkipIntro()
    {
        if (isSkipped) return; // evite un double-clic
        isSkipped = true;

        if (sequenceCoroutine != null)
            StopCoroutine(sequenceCoroutine);

        SceneManager.LoadScene(nextSceneName);
    }

    IEnumerator PlaySequence()
    {
        foreach (var p in panels)
        {
            if (p.canvasGroup == null)
            {
                Debug.LogWarning("Un panel n'a pas de CanvasGroup assigne, il sera ignore.");
                continue;
            }

            p.canvasGroup.gameObject.SetActive(true);

            // Fondu d'entree (0 -> 1)
            yield return StartCoroutine(Fade(p.canvasGroup, 0f, 1f, p.fadeDuration));

            // Affichage a pleine opacite pendant la duree demandee
            yield return new WaitForSeconds(p.displayDuration);

            // Fondu de sortie (1 -> 0)
            yield return StartCoroutine(Fade(p.canvasGroup, 1f, 0f, p.fadeDuration));

            p.canvasGroup.gameObject.SetActive(false);
        }

        // Tous les panels sont passes, on charge la scene de connexion
        SceneManager.LoadScene(nextSceneName);
    }

    IEnumerator Fade(CanvasGroup cg, float from, float to, float duration)
    {
        float elapsed = 0f;
        cg.alpha = from;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            cg.alpha = Mathf.Lerp(from, to, elapsed / duration);
            yield return null;
        }

        cg.alpha = to;
    }
}