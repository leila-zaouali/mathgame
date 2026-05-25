using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeBackgroundScene2 : MonoBehaviour
{
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "1game")
        {
            // 🎮 Scene 2 → fond noir
            Camera.main.clearFlags = CameraClearFlags.SolidColor;
            Camera.main.backgroundColor = Color.black;
        }
        else if (scene.name == "intro")
        {
            // 🌤️ Scene 1 → fond normal (skybox)
            Camera.main.clearFlags = CameraClearFlags.Skybox;
        }
    }
}