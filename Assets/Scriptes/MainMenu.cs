using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string levelToLoad = "MainLevel";

    public SceneFader sceneFader;

    public void Play()
    {
        sceneFader.FadeTo(levelToLoad);
    }

    public void RestScore()
    {
        PlayerPrefs.DeleteKey("HighScore");
    }

    public void Quit()
    {
        Debug.Log("Exeting...");
        Application.Quit();
    }
}
