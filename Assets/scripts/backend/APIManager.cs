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
    public static bool progressLoaded = false; // ✅ NOUVEAU

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
    public void Login(string username, string password, System.Action<bool, string> callback)
    {
        StartCoroutine(LoginRequest(username, password, callback));
    }

    IEnumerator LoginRequest(string username, string password, System.Action<bool, string> callback)
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

            if (!data.success)
            {
                callback(false, data.message);
                yield break;
            }

            callback(true, "");

            PlayerSession.UserId = data.userId;
            Debug.Log("✅ USER ID = " + PlayerSession.UserId);

            // ✅ Reset flag avant de charger
            APIManager.progressLoaded = false;
            APIManager.nextScene = "intro1";

            SceneManager.LoadScene("LoadingScene");

            while (ScoreManager.instance == null) yield return null;

            GetProgress();
        }
        else
        {
            callback(false, "Erreur de connexion !");
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
            Debug.Log("LEVEL UPDATED: " + request.downloadHandler.text);
        else
            Debug.Log("ERROR LEVEL UPDATE: " + request.error);
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

            ProgressResponse data = JsonUtility.FromJson<ProgressResponse>(request.downloadHandler.text);

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

            restoreInventory = (data.progress != null && data.progress.game1);

            // INVENTORY
            if (!string.IsNullOrEmpty(data.inventory))
                StartCoroutine(WaitAndRestoreInventory(data.inventory));

            // CHECKPOINT
            if (data.checkpoint != null && !string.IsNullOrEmpty(data.checkpoint.scene))
            {
                if (ProgressManager.instance != null)
                    ProgressManager.instance.SetCheckpoint(
                        data.checkpoint.scene,
                        data.checkpoint.x,
                        data.checkpoint.y,
                        data.checkpoint.z
                    );

                APIManager.nextScene = data.checkpoint.scene;
                Debug.Log("✅ CHECKPOINT RESTAURÉ: " + data.checkpoint.scene);
            }
            else
            {
                Debug.Log("📍 PAS DE CHECKPOINT → intro1");
            }

            // ✅ Progress chargé — autoriser le spawn
            APIManager.progressLoaded = true;
        }
    }

    IEnumerator WaitAndRestoreInventory(string inventoryData)
    {
        while (Inventory.instance == null) yield return null;
        Inventory.instance.RestoreInventoryFromServer(inventoryData);
    }

    // =====================
    // SAVE INVENTORY
    // =====================
    public void SaveInventory(string inventoryData)
    {
        StartCoroutine(SaveInventoryRequest(inventoryData));
    }

    IEnumerator SaveInventoryRequest(string inventoryData)
    {
        if (string.IsNullOrEmpty(PlayerSession.UserId))
        {
            Debug.Log("❌ SaveInventory blocked: UserId NULL");
            yield break;
        }

        string url = baseUrl + "/saveInventory";
        string json = "{\"userId\":\"" + PlayerSession.UserId + "\",\"inventory\":\"" + inventoryData + "\"}";

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] body = System.Text.Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(body);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();
        Debug.Log("SAVE INVENTORY: " + request.downloadHandler.text);
    }

    // =====================
    // RESET PROGRESS
    // =====================
    public void ResetProgress()
    {
        StartCoroutine(ResetProgressRequest());
    }

    IEnumerator ResetProgressRequest()
    {
        string url = baseUrl + "/resetProgress";
        string json = "{\"userId\":\"" + PlayerSession.UserId + "\"}";

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] body = System.Text.Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(body);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();
        Debug.Log("RESET: " + request.downloadHandler.text);
    }

    // =====================
    // SAVE CHECKPOINT
    // =====================
    public void SaveCheckpoint(string scene, Vector3 pos)
    {
        StartCoroutine(SaveCheckpointRequest(scene, pos));
    }

    IEnumerator SaveCheckpointRequest(string scene, Vector3 pos)
    {
        if (string.IsNullOrEmpty(PlayerSession.UserId)) yield break;

        // ✅ Forcer le point comme séparateur décimal
        string x = pos.x.ToString(System.Globalization.CultureInfo.InvariantCulture);
        string y = pos.y.ToString(System.Globalization.CultureInfo.InvariantCulture);
        string z = pos.z.ToString(System.Globalization.CultureInfo.InvariantCulture);

        string url = baseUrl + "/saveCheckpoint";
        string json = "{\"userId\":\"" + PlayerSession.UserId + "\"," +
                      "\"scene\":\"" + scene + "\"," +
                      "\"x\":" + x + "," +
                      "\"y\":" + y + "," +
                      "\"z\":" + z + "}";

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] body = System.Text.Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(body);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();
        Debug.Log("SAVE CHECKPOINT: " + request.downloadHandler.text);
    }
}