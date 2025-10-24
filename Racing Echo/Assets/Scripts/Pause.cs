using UnityEngine;
using LogitechG29.Sample.Input;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    [SerializeField] InputControllerReader inputControllerReader;
    [SerializeField] GameObject pause;
    [SerializeField] AudioSource audioSource;
    void Update()
    {
        if (inputControllerReader.Options)
        {
            audioSource.mute = true;
            Time.timeScale = 0;
            pause.SetActive(true);
        }
    }
    public void Continue()
    {
        pause.SetActive(false);
        Time.timeScale = 1;
        audioSource.mute = false;
    }
    public void Menu()
    {
        SceneManager.LoadScene("Menu");
    }
}
