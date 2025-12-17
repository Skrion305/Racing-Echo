using UnityEngine;
using LogitechG29.Sample.Input;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    [SerializeField] InputControllerReader inputControllerReader;
    [SerializeField] GameObject pause;
    [SerializeField] AudioSource audioSource;
    [SerializeField] GameModes race;
    [SerializeField] GameObject raceTimer;
    [SerializeField] GameObject timer;
    void Update()
    {
        if (inputControllerReader.Options)
        {
            audioSource.mute = true;
            Time.timeScale = 0;
            if ((GameData.gameMode == "Гонка") && (!race.start))
            {
                raceTimer.SetActive(false);
            }
            if (GameData.gameMode == "На время")
            {
                timer.SetActive(false);
            }
            pause.SetActive(true);
        }
    }
    public void Continue()
    {
        pause.SetActive(false);
        if ((GameData.gameMode == "Гонка") && (!race.start))
        {
            raceTimer.SetActive(true);
        }
        if (GameData.gameMode == "На время")
        {
            timer.SetActive(true);
        }
        Time.timeScale = 1;
        audioSource.mute = false;
    }
    public void Menu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
}
