using UnityEngine;
using UnityEngine.SceneManagement;

public class Scenegame1 : MonoBehaviour
{
    public string sceneName = "Scene2";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(sceneName);
            Destroy(gameObject);
        }
    }
}