using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    public TMP_Text scoreText;

    void Update()
    {
        if (ScoreManager.instance == null) return;

        scoreText.text = ""+ ScoreManager.instance.currentScore;
    }
}