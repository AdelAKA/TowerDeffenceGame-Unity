using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject ui;

    public SceneFader sceneFader;

    public string menuSceneName = "MainMenu";

    [Header("Sound Stuff")]
    public AudioSource audioPlayer;
    public Image muteImage;
    public bool isMute = false;
    public Sprite soundOnSprite;
    public Sprite soundOffSprite;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            Toggle();
        }
    }

    public void Toggle()
    {
        ui.SetActive(!ui.activeSelf);

        if (ui.activeSelf)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }

    public void Retry()
    {
        Toggle();
        sceneFader.FadeTo(SceneManager.GetActiveScene().name);
    }

    public void Menu()
    {
        Toggle();
        sceneFader.FadeTo(menuSceneName);
    }

    public void ToggleMute()
    {
        isMute = !isMute;
        if (isMute)
        {
            muteImage.sprite = soundOffSprite;
            audioPlayer.Pause();
        }
        else
        {
            muteImage.sprite = soundOnSprite;
            audioPlayer.UnPause();
        }

    }
}
