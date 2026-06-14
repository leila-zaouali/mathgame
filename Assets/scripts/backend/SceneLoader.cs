using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneLoader : MonoBehaviour
{
    IEnumerator Start()
    {
        yield return new WaitForSeconds(1f);

        string targetScene = "intro1";

        string id = PlayerSession.UserId;

        if (PlayerPrefs.GetInt(id + "_cp_active", 0) == 1)
        {
            targetScene = PlayerPrefs.GetString(id + "_cp_scene");
            Debug.Log("🚀 Chargement checkpoint : " + targetScene);
        }
        else
        {
            Debug.Log("🚀 PAS DE CHECKPOINT → intro1");
        }

        SceneManager.LoadScene(targetScene);
    }
}