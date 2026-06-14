using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScene : MonoBehaviour
{
    IEnumerator Start()
    {
        yield return new WaitForSeconds(1f);

        string scene = APIManager.nextScene;

        AsyncOperation op = SceneManager.LoadSceneAsync(scene);

        while (!op.isDone)
            yield return null;
    }
}