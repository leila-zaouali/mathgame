using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class APIManager : MonoBehaviour
{
    public static APIManager instance;
    private string baseUrl = "http://127.0.0.1:3000";
    public static bool restoreInventory;
    public static string nextScene;
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
        Debug.Log("APIManager Awake");
    }

    // =====================
    // REGISTER
    // =====================
    public void Register(string username, string password)
    {
        StartCoroutine(RegisterRequest(username, password));
    }

    IEnumerator RegisterRequest(string username, string password)
    {
        string url = baseUrl + "/register";
        string json = "{\"username\":\"" + username + "\",\"password\":\"" + password + "\"}";

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] body = System.Text.Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(body);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();
        Debug.Log("REGISTER: " + request.downloadHandler.text);
    }

    // =====================
    // LOGIN
    // =====================
    public void Login(string username, string password)
    {
        StartCoroutine(LoginRequest(username, password));
    }

    IEnumerator LoginRequest(string username, string password)
    {
        string url = baseUrl + "/login";
        string json = "{\"username\":\"" + username + "\",\"password\":\"" + password + "\"}";

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] body = System.Text.Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(body);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string response = request.downloadHandler.text;
            Debug.Log("RAW LOGIN RESPONSE: " + response);
            LoginResponse data = JsonUtility.FromJson<LoginResponse>(response);

            // USER ID ONLY
            PlayerSession.UserId = data.userId;
            Debug.Log("✅ USER ID = " + PlayerSession.UserId);
            string id = PlayerSession.UserId;
            // LOAD SCENE FIRST
            if (PlayerPrefs.GetInt(id + "_cp_active", 0) == 1)
            {
                APIManager.nextScene = PlayerPrefs.GetString(id + "_cp_scene");
            }
            else
            {
                APIManager.nextScene = "intro1";
            }

            SceneManager.LoadScene("LoadingScene");
            
            // WAIT MANAGERS READY
            while (ScoreManager.instance == null) yield return null;

            // GET FULL DATA FROM SERVER
            GetProgress();
        }
    }

    // =====================
    // SAVE SCORE
    // =====================
    public void SaveScore(int scoreToAdd)
    {
        StartCoroutine(SaveScoreRequest(scoreToAdd));
    }

    IEnumerator SaveScoreRequest(int scoreToAdd)
    {
        if (string.IsNullOrEmpty(PlayerSession.UserId))
        {
            Debug.Log("❌ SaveScore blocked: UserId NULL");
            yield break;
        }

        string url = baseUrl + "/saveScore";
        string json = "{\"userId\":\"" + PlayerSession.UserId + "\",\"score\":" + scoreToAdd + "}";
        Debug.Log("SEND SCORE JSON: " + json);

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] body = System.Text.Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(body);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();
        Debug.Log("RESPONSE: " + request.downloadHandler.text);
    }

    // =====================
    // PING TEST
    // =====================
    public void Ping()
    {
        StartCoroutine(PingRequest());
    }

    IEnumerator PingRequest()
    {
        UnityWebRequest request = UnityWebRequest.Get(baseUrl + "/ping");
        yield return request.SendWebRequest();
        Debug.Log("PING: " + request.downloadHandler.text);
    }

    IEnumerator SetScoreNextFrame(int score)
    {
        yield return new WaitForSeconds(0.2f);
        ScoreManager.instance.SetScore(score);
    }

    // =====================
    // SAVE PROGRESS
    // =====================
    public void SaveProgress(string puzzleName, bool value)
    {
        StartCoroutine(SaveProgressRequest(puzzleName, value));
    }

    IEnumerator SaveProgressRequest(string puzzleName, bool value)
    {
        if (string.IsNullOrEmpty(PlayerSession.UserId))
        {
            Debug.Log("❌ SaveProgress blocked: UserId NULL");
            yield break;
        }

        string url = baseUrl + "/saveProgress";
        string json = "{\"userId\":\"" + PlayerSession.UserId + "\",\"puzzleName\":\"" + puzzleName + "\",\"value\":" + value.ToString().ToLower() + "}";

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] body = System.Text.Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(body);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();
        Debug.Log(request.downloadHandler.text);
    }

    // =====================
    // UPDATE LEVEL
    // =====================
    public void UpdateLevel()
    {
        StartCoroutine(UpdateLevelRequest());
    }

    IEnumerator UpdateLevelRequest()
    {
        if (string.IsNullOrEmpty(PlayerSession.UserId))
        {
            Debug.Log("❌ UpdateLevel blocked: UserId NULL");
            yield break;
        }

        string url = baseUrl + "/updateLevel";
        string json = "{\"userId\":\"" + PlayerSession.UserId + "\"}";

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] body = System.Text.Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(body);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("LEVEL UPDATED: " + request.downloadHandler.text);
        }
        else
        {
            Debug.Log("ERROR LEVEL UPDATE: " + request.error);
        }
    }

    // =====================
    // GET PROGRESS
    // =====================
    public void GetProgress()
    {
        StartCoroutine(GetProgressRequest());
    }
    IEnumerator GetProgressRequest()
    {
        string url = baseUrl + "/getProgress/" + PlayerSession.UserId;
        UnityWebRequest request = UnityWebRequest.Get(url);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("PROGRESS: " + request.downloadHandler.text);

            ProgressResponse data =
                JsonUtility.FromJson<ProgressResponse>(request.downloadHandler.text);

            // SCORE
            if (ScoreManager.instance != null)
                ScoreManager.instance.SetScore(data.score);

            // LEVEL
            Debug.Log("LEVEL = " + data.level);

            // PROGRESS
            Debug.Log("GAME1 = " + data.progress.game1);
            Debug.Log("LAMP = " + data.progress.lamp_puzzle);

            if (ProgressManager.instance != null)
            {
                ProgressManager.instance.game1Completed = data.progress.game1;
                ProgressManager.instance.lampPuzzleCompleted = data.progress.lamp_puzzle;
            }

            // ✅ IMPORTANT : juste stocker l’info
            restoreInventory = (data.progress != null && data.progress.game1);
        }
    }
    IEnumerator WaitInventoryAndRestore()
    {
        while (Inventory.instance == null) yield return null;
        Inventory.instance.SetWaterDropFromSave();
    }
}
