[System.Serializable]
public class LoginResponse
{
    public bool success;
    public string userId;
    public int score;
    public int level;
    public string[] progress;
}

[System.Serializable]
public class UserData
{
    public string username;
    public string password;
    public int score;
    public int level;
    public string[] progress;
}