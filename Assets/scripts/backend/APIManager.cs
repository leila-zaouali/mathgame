using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class APIManager : MonoBehaviour
{
    public static APIManager instance;

    string baseUrl = "http://127.0.0.1:3000";

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

            LoginResponse data = JsonUtility.FromJson<LoginResponse>(response);

            PlayerSession.UserId = data.userId;

            SceneManager.LoadScene("intro1");

            yield return new WaitForSeconds(0.2f); // 🔥 IMPORTANT

            while (ScoreManager.instance == null)
                yield return null;

            ScoreManager.instance.SetScore(data.user.score);

            Debug.Log("📥 SCORE SET = " + data.user.score);
        }
    }

    // =====================
    // SAVE SCORE (BF28)
    // =====================
    public void SaveScore(int scoreToAdd)
    {
        StartCoroutine(SaveScoreRequest(scoreToAdd));
    }

    IEnumerator SaveScoreRequest(int scoreToAdd)
    {
        string url = baseUrl + "/saveScore";

        string json =
            "{\"userId\":\"" + PlayerSession.UserId +
            "\",\"scoreToAdd\":" + scoreToAdd + "}";

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
    public void SaveProgress(string puzzleName, bool value)
    {
        StartCoroutine(SaveProgressRequest(puzzleName, value));
    }

    IEnumerator SaveProgressRequest(string puzzleName, bool value)
    {
        string url = baseUrl + "/saveProgress";

        string json =
            "{\"userId\":\"" + PlayerSession.UserId +
            "\",\"puzzleName\":\"" + puzzleName +
            "\",\"value\":" + value.ToString().ToLower() + "}";

        UnityWebRequest request =
            new UnityWebRequest(url, "POST");

        byte[] body =
            System.Text.Encoding.UTF8.GetBytes(json);

        request.uploadHandler =
            new UploadHandlerRaw(body);

        request.downloadHandler =
            new DownloadHandlerBuffer();

        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        Debug.Log(request.downloadHandler.text);
    }
    public void UpdateLevel()
    {
        StartCoroutine(UpdateLevelRequest());
    }
    IEnumerator UpdateLevelRequest()
    {
        string url = baseUrl + "/updateLevel";

        string json =
            "{\"userId\":\"" + PlayerSession.UserId + "\"}";

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
}

