using UnityEngine;
using UnityEngine.SceneManagement; // Obligatoire pour intercepter les changements de scène

public class ResetPlayerScale : MonoBehaviour
{
    private void OnEnable()
    {
        // Indique à Unity d'exécuter notre fonction à chaque chargement de scène
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        // Nettoie l'écouteur quand l'objet est détruit pour éviter les bugs
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Cette fonction s'exécute AUTOMATIQUEMENT à chaque changement de scène, même au retour
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameObject joueur = GameObject.FindWithTag("Player");

        if (joueur != null)
        {
            // Applique la taille par défaut spécifique (1, 1.904, 1)
            joueur.transform.localScale = new Vector3(1f, 1.904f, 1f);
            Debug.Log("📏 Taille du joueur réinitialisée à (1, 1.904, 1) dans : " + scene.name);
        }
    }
}
