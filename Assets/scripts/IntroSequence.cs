using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable]
public class IntroPanel
{
    public CanvasGroup canvasGroup;
    public float displayDuration = 3f;
    public float fadeDuration = 1f;
}

public class IntroSequence : MonoBehaviour
{
    [Tooltip("Vos 2 premiers panels uniquement (Taille: 2 dans l'inspecteur)")]
    public IntroPanel[] panels;

    [Tooltip("Glissez ici votre Panel (2)")]
    public GameObject troisiemePanelTextes;

    public string nextSceneName = "LoginScene";
    public Button skipButton;

    private bool isSkipped = false;
    private Coroutine sequenceCoroutine;

    void Start()
    {
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
        if (isSkipped) return;
        isSkipped = true;

        if (sequenceCoroutine != null)
            StopCoroutine(sequenceCoroutine);

        FinaliserIntroEtPasserLaMain();
    }

    IEnumerator PlaySequence()
    {
        foreach (var p in panels)
        {
            if (p.canvasGroup == null) continue;

            p.canvasGroup.gameObject.SetActive(true);
            yield return StartCoroutine(Fade(p.canvasGroup, 0f, 1f, p.fadeDuration));
            yield return new WaitForSeconds(p.displayDuration);
            yield return StartCoroutine(Fade(p.canvasGroup, 1f, 0f, p.fadeDuration));
            p.canvasGroup.gameObject.SetActive(false);
        }

        FinaliserIntroEtPasserLaMain();
    }

    private void FinaliserIntroEtPasserLaMain()
    {
        // On détache ce script du bouton Skip pour libérer le bouton
        if (skipButton != null)
            skipButton.onClick.RemoveListener(SkipIntro);

        if (troisiemePanelTextes != null)
        {
            troisiemePanelTextes.SetActive(true);
        }
        else
        {
            SceneManager.LoadScene(nextSceneName);
        }

        // Éteint ce script pour qu'il n'interfère plus jamais
        this.enabled = false;
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
