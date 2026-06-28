using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TextSequenceManager : MonoBehaviour
{
    [System.Serializable]
    public class TexteMinuterie
    {
        public GameObject objetTexte;
        public float tempsAffichage = 10.0f;
    }

    [Header("Configuration des Textes")]
    [SerializeField] private TexteMinuterie[] sequenceTextes;

    [Header("Configuration du Bouton")]
    [SerializeField] private Button boutonSkip;

    [Header("Scène Suivante")]
    [SerializeField] private string nomSceneLogin = "LoginScene";

    private int indexActuel = 0;
    private Coroutine coroutineDefilement;

    // Utiliser OnEnable garantit que le code se lance PILE au moment où le panel s'allume
    private void OnEnable()
    {
        indexActuel = 0;

        foreach (var t in sequenceTextes)
        {
            if (t.objetTexte != null) t.objetTexte.SetActive(false);
        }

        if (boutonSkip != null)
        {
            boutonSkip.onClick.RemoveAllListeners();
            boutonSkip.onClick.AddListener(ClicBoutonSkip);
        }

        if (sequenceTextes.Length > 0)
        {
            coroutineDefilement = StartCoroutine(GestionDefilementAutomatique());
        }
    }

    private IEnumerator GestionDefilementAutomatique()
    {
        while (indexActuel < sequenceTextes.Length)
        {
            if (sequenceTextes[indexActuel].objetTexte != null)
                sequenceTextes[indexActuel].objetTexte.SetActive(true);

            yield return new WaitForSeconds(sequenceTextes[indexActuel].tempsAffichage);

            if (sequenceTextes[indexActuel].objetTexte != null)
                sequenceTextes[indexActuel].objetTexte.SetActive(false);

            indexActuel++;
        }

        PasserALaSceneSuivante();
    }

    public void ClicBoutonSkip()
    {
        if (coroutineDefilement != null)
            StopCoroutine(coroutineDefilement);

        if (indexActuel < sequenceTextes.Length && sequenceTextes[indexActuel].objetTexte != null)
        {
            sequenceTextes[indexActuel].objetTexte.SetActive(false);
        }

        indexActuel++;

        if (indexActuel < sequenceTextes.Length)
        {
            coroutineDefilement = StartCoroutine(GestionDefilementAutomatique());
        }
        else
        {
            PasserALaSceneSuivante();
        }
    }

    private void PasserALaSceneSuivante()
    {
        SceneManager.LoadScene(nomSceneLogin);
    }
}

