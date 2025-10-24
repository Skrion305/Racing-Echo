using UnityEngine;
using UnityEngine.SceneManagement;
using Bhaptics.SDK2;

public class Menu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("SampleScene");
    }
    public void ExitGame()
    {
        //Application.Quit();
        UnityEditor.EditorApplication.isPlaying = false;
    }
    public void PlayUIButtonClick()
    {
        BhapticsLibrary.Play(eventId: "ui_button_click", startMillis: 0, intensity: 0.6f, duration: 150, angleX: 0, offsetY: 0);
    }
}
