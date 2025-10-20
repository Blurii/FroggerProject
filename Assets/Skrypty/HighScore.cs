using UnityEngine;
using UnityEngine.UI;

public class HighScoreMenu : MonoBehaviour
{
    public Text highScoreText;
    public Button resetButton;
    void Start()
    {
        LoadHighScores();
    }
    void LoadHighScores()
    {
        string highScores = PlayerPrefs.GetString("HighScores", "");
        string[] entries = highScores.Split('|');
        var scoreList = new System.Collections.Generic.List<(string playerName, int score)>();
        foreach (string entry in entries)
        {
            if (!string.IsNullOrEmpty(entry))
            {
                string[] parts = entry.Split(':');
                if (parts.Length == 2)
                {
                    string playerName = parts[0];
                    int score = int.Parse(parts[1]);
                    scoreList.Add((playerName, score));
                }
            }
        }

        scoreList.Sort((x, y) => y.score.CompareTo(x.score));
        highScoreText.text = "";
        foreach (var score in scoreList)
        {
            highScoreText.text += $"{score.playerName}: {score.score}\n";
        }
    }

    public void ResetHighScores()
    {
        PlayerPrefs.DeleteKey("HighScores");
        highScoreText.text = "Wyniki zosta³y zresetowane!";
        LoadHighScores();
    }
}
