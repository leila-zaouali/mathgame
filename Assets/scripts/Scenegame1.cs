using UnityEngine;
using UnityEngine.SceneManagement;

public class Scenegame1 : MonoBehaviour
{
    public string sceneName = "Scene2";

    private static bool used = false;

    private void OnTriggerEnter(Collider other)
    {
        if (used) return;

        if (other.CompareTag("Player"))
        {
            used = true;

            Debug.Log("➡ Changement de scène vers " + sceneName);

            SceneManager.LoadScene(sceneName);

            Destroy(gameObject);
        }
    }
}