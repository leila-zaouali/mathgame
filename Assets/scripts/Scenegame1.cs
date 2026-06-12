using UnityEngine;
using UnityEngine.SceneManagement;

public class Scenegame1 : MonoBehaviour
{
    public string sceneName = "Scene2";

    void Start()
    {
        if (ProgressManager.instance != null &&
            ProgressManager.instance.game1Completed)
        {
            Debug.Log("💧 Game1 déjà terminé");

            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("➡ Changement de scène vers " + sceneName);

            SceneManager.LoadScene(sceneName);
        }
    }
}