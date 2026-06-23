using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Scenegame1 : MonoBehaviour
{
    public string sceneName = "Scene2";

    [Header("Parametres du Retrecissement")]
    public float dureeRetrecissement = 2.0f;
    public Vector3 tailleAtome = new Vector3(0.05f, 0.05f, 0.05f);

    [Header("Parametres Audio")]
    public AudioSource audioSource;
    public AudioClip sonRetrecissement;

    private bool aEteTouche = false;

    void Start()
    {
        StartCoroutine(CheckIfAlreadyCompleted());
    }

    IEnumerator CheckIfAlreadyCompleted()
    {
        // ✅ On attend que le flag global de chargement de progression soit pret.
        // Tant que APIManager.progressLoaded est false, on ne sait pas encore
        // si le puzzle a deja ete complete ou non -> on patiente.
        while (!APIManager.progressLoaded)
        {
            yield return null;
        }

        if (ProgressManager.instance != null && ProgressManager.instance.game1Completed)
        {
            Debug.Log("💧 Game1 deja termine -> cube desactive");
            gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("⚛ Game1 pas encore termine -> cube actif");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !aEteTouche)
        {
            aEteTouche = true;
            StartCoroutine(SequenceRetrecissementEtChargement(other.gameObject));
        }
    }

    IEnumerator SequenceRetrecissementEtChargement(GameObject joueur)
    {
        Debug.Log("⚛ Entree dans le monde des atomes : Retrecissement en cours...");

        if (audioSource != null && sonRetrecissement != null)
        {
            audioSource.PlayOneShot(sonRetrecissement);
        }

        Vector3 tailleInitiale = joueur.transform.localScale;
        float tempsEcoule = 0f;

        while (tempsEcoule < dureeRetrecissement)
        {
            tempsEcoule += Time.deltaTime;
            float progression = tempsEcoule / dureeRetrecissement;
            joueur.transform.localScale = Vector3.Lerp(tailleInitiale, tailleAtome, progression);
            yield return null;
        }

        joueur.transform.localScale = tailleAtome;
        Debug.Log("➡ Changement de scene vers " + sceneName);
        SceneManager.LoadScene(sceneName);
    }
}