using UnityEngine;


public class GameProgressManager : MonoBehaviour
{
    public static GameProgressManager instance;

    public bool lampDone;
    public bool voltmeterDone;
    public bool thalesDone;

    void Awake()
    {
        instance = this;
    }

    public void CheckLevelUp()
    {
        if (lampDone && voltmeterDone && thalesDone)
        {
            Debug.Log("?? LEVEL COMPLETE");

            if (APIManager.instance != null)
                APIManager.instance.UpdateLevel();
        }
    }
}
