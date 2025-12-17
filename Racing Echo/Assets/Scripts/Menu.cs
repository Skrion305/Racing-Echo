using UnityEngine;
using UnityEngine.SceneManagement;
using Bhaptics.SDK2;
using UnityEngine.UI;
using TMPro;

public class Menu : MonoBehaviour
{
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject settings;
    [SerializeField] GameObject achievements;
    [SerializeField] GameObject gameModes;
    [SerializeField] GameObject raceTracks;
    [SerializeField] Slider slider;
    [SerializeField] TMP_Text achievement1;
    [SerializeField] TMP_Text achievement2;
    void Start()
    {
        AudioListener.volume = GameData.volume;
        slider.value = GameData.volume;
        if (GameData.achievement1)
        {
            achievement1.color = new Color32(22, 123, 219, 255);
        }
        if (GameData.achievement2)
        {
            achievement2.color = new Color32(22, 123, 219, 255);
        }
    }
    public void StartGame()
    {
        mainMenu.SetActive(false);
        gameModes.SetActive(true);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    public void PlayUIButtonClick()
    {
        BhapticsLibrary.Play(eventId: "ui_button_click", startMillis: 0, intensity: 0.6f, duration: 150, angleX: 0, offsetY: 0);
    }
    public void Settings()
    {
        mainMenu.SetActive(false);
        settings.SetActive(true);
    }
    public void Achievements()
    {
        mainMenu.SetActive(false);
        achievements.SetActive(true);
    }
    public void Volume(float value)
    {
        GameData.volume = value;
        AudioListener.volume = value;
    }
    public void MainMenu()
    {
        gameModes.SetActive(false);
        settings.SetActive(false);
        achievements.SetActive(false);
        mainMenu.SetActive(true);
    }
    public void GameMode1()
    {
        GameData.gameMode = "Гонка";
        gameModes.SetActive(false);
        raceTracks.SetActive(true);
    }
    public void GameMode2()
    {
        GameData.gameMode = "На время";
        gameModes.SetActive(false);
        raceTracks.SetActive(true);
    }
    public void GameMode3()
    {
        GameData.gameMode = "Свободный заезд";
        gameModes.SetActive(false);
        raceTracks.SetActive(true);
    }
    public void Track1()
    {
        GameData.raceTrack = 1;
        SceneManager.LoadScene("SampleScene");
    }
    public void Track2()
    {
        GameData.raceTrack = 2;
        SceneManager.LoadScene("SampleScene");
    }
    public void Track3()
    {
        GameData.raceTrack = 3;
        SceneManager.LoadScene("SampleScene");
    }
    public void Back()
    {
        raceTracks.SetActive(false);
        gameModes.SetActive(true);
    }
}
