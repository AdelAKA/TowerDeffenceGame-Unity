using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public Text roundsText;
    public Text highScoreText;
    public Text newHightScoreText;

    public SceneFader sceneFader;

    public string menuSceneName = "MainMenu";

    void Start()
    {
        highScoreText.text = PlayerPrefs.GetInt("HighScore", 0).ToString();
    }

    void OnEnable()
    {
        roundsText.text = PlayerStatus.Rounds.ToString();
        newHightScoreText.text = "Highest Score";
        if (PlayerStatus.Rounds >= PlayerPrefs.GetInt("HighScore", 0))
        {
            PlayerPrefs.SetInt("HighScore", PlayerStatus.Rounds);
            highScoreText.text = PlayerStatus.Rounds.ToString();
            newHightScoreText.text = "New High Score!";
        }
    }

    public void Retry()
    {
        sceneFader.FadeTo(SceneManager.GetActiveScene().name);
    }

    public void Menu()
    {
        sceneFader.FadeTo(menuSceneName);
    }
}
