using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Scenegame1 : MonoBehaviour
{
    public string sceneName = "Scene2";

    [Header("Paramètres du Rétrécissement")]
    public float dureeRetrecissement = 2.0f;
    public Vector3 tailleAtome = new Vector3(0.05f, 0.05f, 0.05f);

    [Header("Paramètres Audio")]
    public AudioSource audioSource; // L'appareil qui joue le son
    public AudioClip sonRetrecissement; // Le fichier audio (.mp3 / .wav)

    private bool aEteTouche = false;

    void Start()
    {
        if (ProgressManager.instance != null && ProgressManager.instance.game1Completed)
        {
            Debug.Log("💧 Game1 déjà terminé");
            gameObject.SetActive(false);
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
        Debug.Log("⚛ Entrée dans le monde des atomes : Rétrécissement en cours...");

        // 🔊 Déclenche le son dès que le joueur touche le cube
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

        Debug.Log("➡ Changement de scène vers " + sceneName);
        SceneManager.LoadScene(sceneName);
    }
}
