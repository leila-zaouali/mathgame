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
            // Force la remise à la taille normale (1, 1, 1)
            joueur.transform.localScale = Vector3.one;
            Debug.Log("📏 Taille du joueur réinitialisée avec succès dans la scène : " + scene.name);
        }
    }
}
