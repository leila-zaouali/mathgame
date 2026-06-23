using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TMP_InputField usernameInput;
    public TMP_InputField passwordInput;

    public APIManager api;

    public TMP_Text debugText;

    // =====================
    // REGISTER BUTTON
    // =====================
    public void OnRegister()
    {
        string username = usernameInput.text;
        string password = passwordInput.text;

        api.Register(username, password);

        debugText.text = "Register sent...";
    }

    // =====================
    // LOGIN BUTTON
    // =====================
    public void OnLogin()
    {
        string username = usernameInput.text;
        string password = passwordInput.text;

        debugText.text = "Connexion...";

        api.Login(username, password, (success, message) =>
        {
            if (!success)
            {
                debugText.text = message; // ? "Joueur non trouvé !"
            }
        });
    }
}