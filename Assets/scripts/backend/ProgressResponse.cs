using System.Collections.Generic;

[System.Serializable]
public class ProgressResponse
{
    public bool success;
    public int level;
    public int score;
    public ProgressData progress;
    public string inventory; // ✅ AJOUTE ÇA

}

[System.Serializable]
public class ProgressData
{
    public bool game1;
    public bool lamp_puzzle;
}

public static class PlayerProgressCache
{
    public static Dictionary<string, bool> progress;
}