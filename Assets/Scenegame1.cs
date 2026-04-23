using UnityEngine;
using UnityEngine.SceneManagement;

public class Scenegame1: MonoBehaviour
{
    public string sceneName = "Scene2";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
            Destroy(gameObject); // 👈 détruit l’objet après utilisation
        }
    }
}