using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class APIManager : MonoBehaviour
{
    string baseUrl = "http://127.0.0.1:3000";

    // =====================
    // ?? REGISTER
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
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);

        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("REGISTER OK: " + request.downloadHandler.text);
        }
        else
        {
            Debug.Log("REGISTER ERROR: " + request.error);
        }
    }

    // =====================
    // ?? LOGIN
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
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);

        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.Success)
        {
            string response = request.downloadHandler.text;
            Debug.Log("LOGIN OK: " + response);

            if (response.Contains("\"success\":true"))
            {
                // 👉 changer de scène
                SceneManager.LoadScene("intro1");
            }
        }
        else
        {
            Debug.Log("LOGIN ERROR: " + request.error);
        }
    }

    // =====================
    // ?? TEST PING
    // =====================
    public void Ping()
    {
        StartCoroutine(PingRequest());
    }

    IEnumerator PingRequest()
    {
        UnityWebRequest request = UnityWebRequest.Get(baseUrl + "/ping");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("PING: " + request.downloadHandler.text);
        }
        else
        {
            Debug.Log("PING ERROR: " + request.error);
        }
    }
}